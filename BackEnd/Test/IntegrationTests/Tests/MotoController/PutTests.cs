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
    public class PutTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Placa Atualizada com Sucesso")]
        public async Task Put_Success()
        {
            Motorcycle motorcycle = MockModels.Motorcycle().First();
            await _integration.Context.Motorcycles.AddAsync(motorcycle);
            await _integration.Context.SaveChangesAsync();

            var jsonContent = JsonConvert.SerializeObject(new { placa = motorcycle.Model });
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientAdm.PutAsync($"/api/v1/motos/{motorcycle.Id}/placa", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.Contains("Placa modificada com sucesso", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Placa não pode ser atualizada")]
        public async Task Put_Fail()
        {
            var motorcycles = MockModels.Motorcycle(2);
            await _integration.Context.Motorcycles.AddRangeAsync(motorcycles);
            await _integration.Context.SaveChangesAsync();

            var jsonContent = JsonConvert.SerializeObject(new { placa = motorcycles.Skip(1).First().Plate });
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientAdm.PutAsync($"/api/v1/motos/{motorcycles.First().Id}/placa", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.Contains("Placa já cadastrada", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }


        [Fact(DisplayName = "Moto não encontrada")]
        public async Task Put_Fail_NotFound()
        {
            var jsonContent = JsonConvert.SerializeObject(new { placa = "placa" });
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientAdm.PutAsync($"/api/v1/motos/{Guid.NewGuid()}/placa", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.Contains("Dados inválidos", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }
    }
}
