using AutoFixture;
using BackEnd;
using BackEnd.DTOs;
using Data.Models;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Test.IntegrationTests.Fixtures;

namespace Test.IntegrationTests.Tests
{
    [Collection(nameof(TestFixtureCollection))]
    public class InfraTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Teste salvar Notificações")]
        public void SaveNotification()
        {
            Notification notification = new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            _integration.Context.Add(notification);
            _integration.Context.SaveChanges();
        }
    }
}
