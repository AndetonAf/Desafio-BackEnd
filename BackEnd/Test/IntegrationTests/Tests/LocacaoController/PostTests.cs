using AutoFixture;
using BackEnd;
using BackEnd.DTOs.EntregadoresController;
using BackEnd.DTOs.LocacaoController;
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

    public class PostTests(TestFixture<Startup> integrationTestFixture)
    {
        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Locado com Sucesso")]
        public async Task Post_Success()
        {
            var user = MockModels.User();
            _integration.Context.Users.Add(user);

            var motorcycle = MockModels.Motorcycle();
            _integration.Context.Motorcycles.AddRange(motorcycle);

            _integration.Context.SaveChanges();

            var locacao = new
            {
                Identificador = Guid.NewGuid().ToString(),
                Entregador_id = user.Id,
                Moto_id = motorcycle.First().Id,
                Data_inicio = DateTime.UtcNow,
                Data_termino = DateTime.UtcNow.AddDays(7),
                Data_previsao_termino = DateTime.UtcNow.AddDays(7),
                Plano = 7,
            };

            var jsonContent = JsonConvert.SerializeObject(locacao);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/locacao", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Created);
        }

        [Fact(DisplayName = "Motorista não tem a CNH A")]
        public async Task Post_Fail_CnhB()
        {
            var user = MockModels.User();
            user.CnhType = CnhType.B;
            _integration.Context.Users.Add(user);

            var motorcycle = MockModels.Motorcycle();
            _integration.Context.Motorcycles.AddRange(motorcycle);

            _integration.Context.SaveChanges();

            var locacao = new
            {
                Identificador = Guid.NewGuid().ToString(),
                Entregador_id = user.Id,
                Moto_id = motorcycle.First().Id,
                Data_inicio = DateTime.UtcNow,
                Data_termino = DateTime.UtcNow.AddDays(7),
                Data_previsao_termino = DateTime.UtcNow.AddDays(7),
                Plano = 7,
            };

            var jsonContent = JsonConvert.SerializeObject(locacao);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/locacao", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.Contains("Motorista não tem a CNH A", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "")]
        public async Task Post_Fail_Id()
        {
            var user = MockModels.User();
            user.CnhType = CnhType.AB;
            _integration.Context.Users.Add(user);

            var motorcycle = MockModels.Motorcycle();
            _integration.Context.Motorcycles.AddRange(motorcycle);

            _integration.Context.SaveChanges();

            var locacao = new
            {
                Identificador = Guid.NewGuid().ToString(),
                Entregador_id = user.Id,
                Moto_id = motorcycle.First().Id,
                Data_inicio = DateTime.UtcNow,
                Data_termino = DateTime.UtcNow.AddDays(7),
                Data_previsao_termino = DateTime.UtcNow.AddDays(7),
                Plano = 7,
            };

            var jsonContent = JsonConvert.SerializeObject(locacao);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/locacao", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            jsonContent = JsonConvert.SerializeObject(locacao);
            contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/locacao", contentString);
            json = await responseRequest.Content.ReadAsStringAsync();

            Assert.Contains("Já cadastrado", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Dados invalidos")]
        public async Task Post_Fail()
        {
            var user = MockModels.User();
            user.CnhType = CnhType.AB;
            _integration.Context.Users.Add(user);

            var motorcycle = MockModels.Motorcycle();
            _integration.Context.Motorcycles.AddRange(motorcycle);

            _integration.Context.SaveChanges();

            var locacao = new RentalDto
            {
                Identificador = Guid.NewGuid().ToString(),
                Entregador_id = Guid.NewGuid().ToString(),
                Moto_id = Guid.NewGuid().ToString(),
                Data_inicio = DateTime.UtcNow,
                Data_termino = DateTime.UtcNow.AddDays(7),
                Data_previsao_termino = DateTime.UtcNow.AddDays(7),
                Plano = 0,
            };

            var jsonContent = JsonConvert.SerializeObject(locacao);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/locacao", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();
            Assert.Contains("Dados inválidos", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);

            locacao.Entregador_id = user.Id;
            jsonContent = JsonConvert.SerializeObject(locacao);
            contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/locacao", contentString);
            json = await responseRequest.Content.ReadAsStringAsync();
            Assert.Contains("Dados inválidos", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);

            locacao.Moto_id = motorcycle.First().Id;
            jsonContent = JsonConvert.SerializeObject(locacao);
            contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/locacao", contentString);
            json = await responseRequest.Content.ReadAsStringAsync();
            Assert.Contains("Dados inválidos", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);

            locacao.Plano = 7;
            jsonContent = JsonConvert.SerializeObject(locacao);
            contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/locacao", contentString);
            json = await responseRequest.Content.ReadAsStringAsync();
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Created);
        }
    }
}
