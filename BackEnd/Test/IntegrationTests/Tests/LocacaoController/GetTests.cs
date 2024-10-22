using AutoFixture;
using BackEnd;
using BackEnd.DTOs.EntregadoresController;
using BackEnd.DTOs.LocacaoController;
using BackEnd.DTOs.MotosController;
using Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.IntegrationTests.Fixtures;
using Test.IntegrationTests.Mock;
using Xunit.Sdk;

namespace Test.IntegrationTests.Tests.LocacaoController
{
    [Collection(nameof(TestFixtureCollection))]

    public class GetTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Obter com Sucesso")]
        public async Task Get_Success()
        {
            var user = MockModels.User();
            _integration.Context.Users.Add(user);
            var motorcycle = MockModels.Motorcycle().First();
            _integration.Context.Motorcycles.Add(motorcycle);

            var rental = MockModels.Rental(user, motorcycle);
            _integration.Context.Rentals.Add(rental);
            _integration.Context.SaveChanges();

            var responseRequest = await _integration.ClientDelivery.GetAsync($"/api/v1/locacao/{rental.Id}");
            var json = await responseRequest.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<RentalGetDto>(json);

            Assert.NotNull(obj);
            Assert.True(obj.Data_inicio == DateTime.UtcNow.Date.AddDays(1));
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Erro ao obter")]
        public async Task Get_Fail()
        {
            var responseRequest = await _integration.ClientDelivery.GetAsync($"/api/v1/locacao/{Guid.NewGuid()}");
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.NotFound);
        }
    }
}
