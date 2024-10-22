using BackEnd.Configurations;
using BackEnd.Services.Interfaces;
using Google.Cloud.PubSub.V1;

namespace BackEnd.Services
{
    public class MessagingService(ILogger<MessagingService> logger) : IMessagingService
    {
        private readonly ILogger _logger = logger;

        public async Task PublishMessagesAsync(IEnumerable<string> messageTexts, string topicId)
        {
            string credential_path_gcp = GlobalConfigurations.Settings.Gcp.PathRelative ? AppDomain.CurrentDomain.BaseDirectory + GlobalConfigurations.Settings.Gcp.PathJson : GlobalConfigurations.Settings.Gcp.PathJson;

            if (!File.Exists(credential_path_gcp))
            {
                return;
            }

            string projectId = GlobalConfigurations.Settings.Gcp.ProjectId;
            
            var topic = TopicName.FromProjectTopic(projectId, topicId);
            PublisherClientBuilder builder = new()
            {
                TopicName = topic,
            };

            PublisherClient publisher = await builder.BuildAsync();

            var publishTasks = messageTexts.Select(async text =>
            {
                string message = await publisher.PublishAsync(text);
            });
            await Task.WhenAll(publishTasks);
        }
    }
}
