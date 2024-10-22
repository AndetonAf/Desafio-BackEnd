using AutoFixture;
using BackEnd;
using BackEnd.DTOs;
using Data.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Test.IntegrationTests.Fixtures;

namespace Test.IntegrationTests.Tests.MotoController
{
    [Collection(nameof(TestFixtureCollection))]
    public class AuthTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Testa Permissão com sucesso")]
        public async Task CheckPermission_Success()
        {
            var responseRequest = await _integration.ClientAdm.GetAsync($"/api/v1/motos");
            Assert.True(responseRequest.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Testa Permissão com falha")]
        public async Task CheckPermission_Fail()
        {
            var responseRequest = await _integration.ClientDelivery.GetAsync($"/api/v1/motos");
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact(DisplayName = "Testa se está logado")]
        public async Task CheckPermission_AuthFail()
        {
            var responseRequest = await _integration.Client.GetAsync($"/api/v1/motos");
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

    }
}
