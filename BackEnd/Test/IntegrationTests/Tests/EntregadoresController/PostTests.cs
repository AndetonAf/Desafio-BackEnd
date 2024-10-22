using AutoFixture;
using BackEnd;
using BackEnd.DTOs;
using BackEnd.DTOs.EntregadoresController;
using Data.Models;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Test.IntegrationTests.Fixtures;
using Test.IntegrationTests.Mock;

namespace Test.IntegrationTests.Tests.EntregadoresController
{
    [Collection(nameof(TestFixtureCollection))]
    public class PostTests(TestFixture<Startup> integrationTestFixture)
    {
        string imgPngBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAMSURBVBhXY/j//z8ABf4C/qc1gYQAAAAASUVORK5CYII=";
        string imgJpgBase64 = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCAABAAEDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD9/KKKKAP/2Q==";

        private readonly TestFixture<Startup> _integration = integrationTestFixture;

        [Fact(DisplayName = "Cadastro com Sucesso")]
        public async Task Post_Success()
        {
            var user = MockModels.UserDto().First();
            user.Imagem_cnh = imgPngBase64;

            var jsonContent = JsonConvert.SerializeObject(user);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Created);
        }

        [Fact(DisplayName = "Cadastro com Erro")]
        public async Task Post_Fail()
        {
            var payload = new
            {
                identificador = "",
                nome = "",
                cnpj = "",
                data_nascimento = "",
                numero_cnh = "",
                tipo_cnh = "",
                imagem_cnh = ""
            };

            var jsonContent = JsonConvert.SerializeObject(payload);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Cnh atualizado com Sucesso")]
        public async Task Post_Cnh_Success()
        {
            var user = MockModels.UserDto().First();
            user.Imagem_cnh = imgPngBase64;

            var jsonContent = JsonConvert.SerializeObject(user);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores", contentString);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Created);

            var userDb = _integration.Context.Users.First(x => x.Name == user.Nome && x.Cnpj == user.Cnpj);

            jsonContent = JsonConvert.SerializeObject(new { imagem_cnh = imgPngBase64 });
            contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores/{userDb.Id}/cnh", contentString);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Created);
        }


        [Fact(DisplayName = "Ext cng imagem invalida")]
        public async Task Post_Extensao_Fail()
        {
            var user = MockModels.UserDto().First();
            user.Imagem_cnh = imgJpgBase64;

            var jsonContent = JsonConvert.SerializeObject(user);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();

            Assert.Contains("Extensão Invalido", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);

            user.Imagem_cnh = imgPngBase64;
            jsonContent = JsonConvert.SerializeObject(user);
            contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores", contentString);
            json = await responseRequest.Content.ReadAsStringAsync();

            var userDb = _integration.Context.Users.First(x => x.Name == user.Nome && x.Cnpj == user.Cnpj);
            jsonContent = JsonConvert.SerializeObject(new { imagem_cnh = imgJpgBase64 });
            contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores/{userDb.Id}/cnh", contentString);
            json = await responseRequest.Content.ReadAsStringAsync();
            Assert.Contains("Extensão Invalido", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);

            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores/{Guid.NewGuid()}/cnh", contentString);
            json = await responseRequest.Content.ReadAsStringAsync();
            Assert.Contains("Dados inválidos", json);
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact(DisplayName = "Evitar cnpj/cnh duplicado")]
        public async Task Post_CnpjCnh_Unico()
        {
            var user = MockModels.UserDto().First();
            user.Imagem_cnh = imgPngBase64;

            var jsonContent = JsonConvert.SerializeObject(user);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Created);

            user.Identificador += Guid.NewGuid();
            jsonContent = JsonConvert.SerializeObject(user);
            contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores", contentString);
            json = await responseRequest.Content.ReadAsStringAsync();
            Assert.Contains("Dados inválidos", json);
        }

        [Fact(DisplayName = "Evitar id duplicado")]
        public async Task Post_Id_Unico()
        {
            var user = MockModels.UserDto().First();
            user.Imagem_cnh = imgPngBase64;

            var jsonContent = JsonConvert.SerializeObject(user);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores", contentString);
            var json = await responseRequest.Content.ReadAsStringAsync();
            Assert.True(responseRequest.StatusCode == System.Net.HttpStatusCode.Created);

            responseRequest = await _integration.ClientDelivery.PostAsync($"/api/v1/entregadores", contentString);
            json = await responseRequest.Content.ReadAsStringAsync();
            Assert.Contains("Já cadastrado", json);
        }
    }
}
