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

    public class PutTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Sucesso na devolução")]
        public async Task Get_Success()
        {
            var user = MockModels.User();
            _integration.Context.Users.Add(user);
            var motorcycle = MockModels.Motorcycle().First();
            _integration.Context.Motorcycles.Add(motorcycle);

            var rental = MockModels.Rental(user, motorcycle);
            _integration.Context.Rentals.Add(rental);
            _integration.Context.SaveChanges();

            var jsonContent = JsonConvert.SerializeObject(new RentalUpdateReturnDateDto { Data_devolucao = rental.StartDate.AddDays(7) });
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientDelivery.PutAsync($"/api/v1/locacao/{rental.Id}/devolucao", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<RentalPutResponseDto>(json);

            Assert.NotNull(obj);
            Assert.True(obj.Values.Days == 7);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact(DisplayName = "Erro ao executar devolução")]
        public async Task Get_Fail()
        {
            var jsonContent = JsonConvert.SerializeObject(new RentalUpdateReturnDateDto { Data_devolucao = DateTime.UtcNow });
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientDelivery.PutAsync($"/api/v1/locacao/{Guid.NewGuid()}/devolucao", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }
    }
}
