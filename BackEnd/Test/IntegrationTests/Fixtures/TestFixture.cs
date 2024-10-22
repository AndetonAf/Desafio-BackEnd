using BackEnd;
using BackEnd.Configurations;
using BackEnd.Services;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Test.IntegrationTests.Fixtures
{
    [CollectionDefinition(nameof(TestFixtureCollection))]
    public class TestFixtureCollection : ICollectionFixture<TestFixture<Startup>> { }

    public class TestFixture<TStartup> : WebApplicationFactory<TStartup>, IDisposable where TStartup : class
    {
        public HttpClient Client;
        public HttpClient ClientAdm;
        public HttpClient ClientDelivery;
        public Context Context;
        private readonly IServiceScope _scope;

        public TestFixture()
        {
            _scope = Services.CreateScope();
            Context = _scope.ServiceProvider.GetRequiredService<Context>();
            Context.Database.Migrate();

            Client = CreateClient();

            ClientAdm = CreateClient();
            ClientAdm.DefaultRequestHeaders.Authorization = SetJwt(UserType.Administrator);

            ClientDelivery = CreateClient();
            ClientDelivery.DefaultRequestHeaders.Authorization = SetJwt(UserType.DeliveryMan);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
        }

        public AuthenticationHeaderValue SetJwt(UserType userType)
        {
            var id = Context.Users.First(x => x.Type == userType).Id.ToString();
            List<Claim> claims = [
                new("sub", id),
                new("type", ((int)userType).ToString())
            ];

            string token = AuthService.GenerateJwt(claims, GlobalConfigurations.Settings.Auth.SecretJwt);

            return new AuthenticationHeaderValue("Bearer", token);
        }

        public new void Dispose()
        {
            Client.Dispose();
            _scope.Dispose();
            base.Dispose();
        }
    }
}
