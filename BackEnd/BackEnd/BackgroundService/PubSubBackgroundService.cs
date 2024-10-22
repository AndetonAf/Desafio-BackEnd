using BackEnd.Configurations;
using BackEnd.DTOs.MessagingService;
using Data;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Newtonsoft.Json;

namespace BackEnd
{
    public class PubSubBackgroundService : BackgroundService
    {
        private readonly ILogger<PubSubBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PubSubBackgroundService(ILogger<PubSubBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string projectId = GlobalConfigurations.Settings.Gcp.ProjectId;
            string credential_path_gcp = GlobalConfigurations.Settings.Gcp.PathRelative ? AppDomain.CurrentDomain.BaseDirectory + GlobalConfigurations.Settings.Gcp.PathJson : GlobalConfigurations.Settings.Gcp.PathJson;

            if (!File.Exists(credential_path_gcp)) { return;  }

            var credential = GoogleCredential.FromFile(credential_path_gcp);

            SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(projectId, "motorcycles-sub");
            SubscriberClientBuilder builder = new SubscriberClientBuilder { Credential = credential, SubscriptionName = subscriptionName, };
            SubscriberClient subscriber = await builder.BuildAsync(stoppingToken);
            Task startTask = subscriber.StartAsync((PubsubMessage message, CancellationToken cancel) =>
            {
                string text = message.Data.ToStringUtf8();
                Console.WriteLine($"Message {message.MessageId}: {text}");

                var dto = JsonConvert.DeserializeObject<IdYearDto>(text);
                if(dto != null)
                {
                    using var scope = _serviceProvider.CreateScope(); // Cria um novo escopo
                    var context = scope.ServiceProvider.GetRequiredService<Context>();
                    if(dto.Year == 2024)
                    {
                        context.Notifications.Add(new Data.Models.Notification("motorcycles-sub", dto.Id));
                        context.SaveChanges();
                    }
                }

                return Task.FromResult(SubscriberClient.Reply.Ack);
            });
            await startTask;

            await subscriber.StopAsync(CancellationToken.None);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}