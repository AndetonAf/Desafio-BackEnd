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

namespace Test.IntegrationTests.Tests.MockAuthController
{
    [Collection(nameof(TestFixtureCollection))]
    public class PostTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact]
        public void SaveNotification()
        {
            Notification notification = new("", "");
        }

        [Fact(DisplayName = "Mock Jwt Administrator")]
        public async Task Get_Administrator()
        {
            var responseRequest = await _integration.Client.GetAsync($"/api/v1/MockAuth/Administrator");
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Mock Jwt DeliveryMan")]
        public async Task Get_DeliveryMan()
        {
            var responseRequest = await _integration.Client.GetAsync($"/api/v1/MockAuth/DeliveryMan");
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.OK);
        }
    }
}
