using BackEnd.DTOs;
using BackEnd.DTOs.EntregadoresController;
using BackEnd.Helpers;
using BackEnd.Services.Interfaces;
using BackEnd.Validators;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BackEnd.Controllers.v1
{
    [Auth(UserType.DeliveryMan)]
    public class EntregadoresController(IFacadeService facadeService) : AbstractV1Controller(facadeService)
    {

        [SwaggerOperation(Summary = "Cadastrar entregador")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MensagemDto))]
        [HttpPost]
        public async Task<IActionResult> Create(UserDto dto)
        {
            if (Facade.UserService.Exist(dto.Identificador))
            {
                return BadRequest(new { mensagem = "Já cadastrado" });

            }
            if (Facade.UserService.ExistCnhCnpj(dto.Numero_cnh, dto.Cnpj))
            {
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            var extension = FileHelpers.DetectExtension(dto.Imagem_cnh);
            if (!extension.success)
            {
                return BadRequest(new { mensagem = "Extensão Invalido" });
            }

            var url = await Facade.StorageService.UploadCnh("cnh", dto.Identificador, extension.contentType, dto.Imagem_cnh);
            Facade.UserService.Create(UserType.DeliveryMan, dto.Identificador, dto, url);

            return StatusCode(201);
        }

        [SwaggerOperation(Summary = "Enviar foto da CNH")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MensagemDto))]
        [HttpPost("{id}/cnh")]
        public async Task<IActionResult> UpdateCnh(string id, [FromBody] UpdateCnhDto dto)
        {
            var userExist = Facade.UserService.Exist(id);
            if(!userExist)
            {
                return BadRequest(new { mensagem = "Dados inválidos" });
            }

            var extension = FileHelpers.DetectExtension(dto.imagem_cnh);
            if (!extension.success)
            {
                return BadRequest(new { mensagem = "Extensão Invalido" });
            }

            var url = await Facade.StorageService.UploadCnh("cnh", id, extension.contentType, dto.imagem_cnh);

            Facade.UserService.UpdateCnhImage(id, url);

            return StatusCode(201);
        }
    }
}
