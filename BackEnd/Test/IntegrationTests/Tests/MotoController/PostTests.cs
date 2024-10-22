using AutoFixture;
using BackEnd;
using BackEnd.DTOs;
using BackEnd.DTOs.MotosController;
using Data.Models;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Test.IntegrationTests.Fixtures;
using Test.IntegrationTests.Mock;

namespace Test.IntegrationTests.Tests.MotoController
{
    [Collection(nameof(TestFixtureCollection))]
    public class PostTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Cadastro com Sucesso")]
        public async Task Post_Success()
        {
            Fixture fixture = new();
            var motorcycle = fixture.Create<MotorcycleDto>();

            var jsonContent = JsonConvert.SerializeObject(motorcycle);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientAdm.PostAsync($"/api/v1/motos", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Created);
        }

        [Fact(DisplayName = "Cadastro com Erro")]
        public async Task Post_Fail()
        {
            var payload = new
            {
                identificador = Guid.NewGuid().ToString(),
                ano = 0,
                modelo = "",
                placa = ""
            };

            var jsonContent = JsonConvert.SerializeObject(payload);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientAdm.PostAsync($"/api/v1/motos", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.Contains("Ano Inválido", json);
            Assert.Contains("Preencha a Placa", json);
            Assert.Contains("Preencha o Modelo", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Não pode cadastrar com placa existente")]
        public async Task Post_ConflitePlate_Success()
        {
            Motorcycle motorcycle = MockModels.Motorcycle(1).First();
            await _integration.Context.Motorcycles.AddAsync(motorcycle);
            await _integration.Context.SaveChangesAsync();

            var payload = new
            {
                identificador = Guid.NewGuid().ToString(),
                ano = 2020,
                modelo = "Moto Sport",
                placa = motorcycle.Plate
            };

            var jsonContent = JsonConvert.SerializeObject(payload);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientAdm.PostAsync($"/api/v1/motos", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.Contains("Placa já cadastrada", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Não pode cadastrar com o mesmo id")]
        public async Task Post_ConfliteId_Success()
        {
            Motorcycle motorcycle = MockModels.Motorcycle(1).First();
            await _integration.Context.Motorcycles.AddAsync(motorcycle);
            await _integration.Context.SaveChangesAsync();

            var payload = new
            {
                identificador = motorcycle.Id,
                ano = 2020,
                modelo = "Moto Sport",
                placa = motorcycle.Plate
            };

            var jsonContent = JsonConvert.SerializeObject(payload);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientAdm.PostAsync($"/api/v1/motos", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.Contains("Já cadastrado", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }
    }
}
