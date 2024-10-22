using AutoFixture;
using BackEnd;
using BackEnd.DTOs;
using BackEnd.DTOs.MotosController;
using Data.Models;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Test.IntegrationTests.Fixtures;
using Test.IntegrationTests.Mock;

namespace Test.IntegrationTests.Tests.MotoController
{
    [Collection(nameof(TestFixtureCollection))]
    public class GetTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Obter lista de motos")]
        public async Task GetAll_Success()
        {
            Motorcycle[] motorcycles = MockModels.Motorcycle(3).ToArray();

            await _integration.Context.Motorcycles.AddRangeAsync(motorcycles);
            await _integration.Context.SaveChangesAsync();

            var responseRequest = await _integration.ClientAdm.GetAsync("/api/v1/motos");
            var json = await responseRequest.Content.ReadAsStringAsync();
            var objs = JsonConvert.DeserializeObject<MotorcycleDto[]>(json);

            Assert.NotNull(objs);
            Assert.All(motorcycles, x => objs.Any(y => x.Year == y.Ano && x.Model == y.Modelo && x.Plate == y.Placa));

            var plateSearch = motorcycles.First().Plate;
            responseRequest = await _integration.ClientAdm.GetAsync($"/api/v1/motos?placa={plateSearch}");
            json = await responseRequest.Content.ReadAsStringAsync();
            objs = JsonConvert.DeserializeObject<MotorcycleDto[]>(json);

            Assert.NotNull(objs);
            Assert.True(objs.Count() > 0);
            Assert.True(objs.All(y => y.Placa == plateSearch));

            _integration.Context.Motorcycles.RemoveRange(motorcycles);
            await _integration.Context.SaveChangesAsync();
        }

        [Fact(DisplayName = "Obter moto por Id")]
        public async Task GetById_Success()
        {
            Motorcycle motorcycle = MockModels.Motorcycle().First();

            await _integration.Context.Motorcycles.AddAsync(motorcycle);
            await _integration.Context.SaveChangesAsync();

            var responseRequest = await _integration.ClientAdm.GetAsync($"/api/v1/motos/{motorcycle.Id}");
            var json = await responseRequest.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<MotorcycleDto>(json);

            Assert.NotNull(obj);
            Assert.True(motorcycle.Year == obj.Ano && motorcycle.Model == obj.Modelo && motorcycle.Plate == obj.Placa);

            _integration.Context.Motorcycles.Remove(motorcycle);
            await _integration.Context.SaveChangesAsync();
        }

        [Fact(DisplayName = "Falha ao Obter moto por Id")]
        public async Task GetById_Fail()
        {
            var responseRequest = await _integration.ClientAdm.GetAsync($"/api/v1/motos/{Guid.NewGuid()}");
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }
    }
}
