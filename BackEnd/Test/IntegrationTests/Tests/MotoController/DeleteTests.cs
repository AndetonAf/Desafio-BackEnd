using AutoFixture;
using BackEnd;
using BackEnd.DTOs;
using Data.Models;
using Newtonsoft.Json;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Test.IntegrationTests.Fixtures;
using Test.IntegrationTests.Mock;

namespace Test.IntegrationTests.Tests.MotoController
{
    [Collection(nameof(TestFixtureCollection))]
    public class DeleteTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Deletado com Sucesso")]
        public async Task Delete_Success()
        {
            Motorcycle motorcycle = MockModels.Motorcycle().First();
            await _integration.Context.Motorcycles.AddAsync(motorcycle);
            await _integration.Context.SaveChangesAsync();

            var responseRequest = await _integration.ClientAdm.DeleteAsync($"/api/v1/motos/{motorcycle.Id.ToString()}");

            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Erro ao deletar")]
        public async Task Delete_Fail()
        {
            var responseRequest = await _integration.ClientAdm.DeleteAsync($"/api/v1/motos/{Guid.NewGuid().ToString()}");
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }
    }
}
