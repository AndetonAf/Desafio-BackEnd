using BackEnd.DTOs;
using BackEnd.DTOs.MotosController;
using BackEnd.Services.Interfaces;
using BackEnd.Validators;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Dapper.SqlMapper;

namespace BackEnd.Controllers.v1
{
    [Auth(UserType.Administrator)]
    public class MotosController(IFacadeService facadeService) : AbstractV1Controller(facadeService)
    {

        [SwaggerOperation(Summary = "Consultar motos existentes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MotorcycleDto[]))]
        [HttpGet]
        public ActionResult<MotorcycleDto[]> Get(string placa = "")
        {
            return Ok(Facade.MotoService.GetAll(placa));
        }

        [SwaggerOperation(Summary = "Consultar motos existentes por id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MotorcycleDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MensagemDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MensagemDto))]
        [HttpGet("{id}")]
        public ActionResult<MotorcycleDto> GetById(string id)
        {
            var motorcycle = Facade.MotoService.GetById(id);
            return motorcycle == null ? BadRequest() : Ok(motorcycle);
        }

        [SwaggerOperation(Summary = "Cadastrar uma nova moto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MensagemDto))]
        [HttpPost]
        public IActionResult Create(MotorcycleDto dto)
        {
            var (success, mensagem) = Facade.MotoService.Create(dto);
            return success ? StatusCode(201) : BadRequest(new { mensagem });
        }

        [SwaggerOperation(Summary = "Modificar a placa de uma moto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MensagemDto))]
        [HttpPut("{id}/placa")]
        public ActionResult<MensagemDto> UpdatePlate(string id, [FromBody] UpdatePlateDto dto)
        {
            var (success, mensagem) = Facade.MotoService.UpdatePlate(id, dto.Placa);
            return success ? Ok(new { mensagem = "Placa modificada com sucesso" }) : BadRequest(new { mensagem });
        }

        [SwaggerOperation(Summary = "Remover uma moto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MensagemDto))]
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var (success, mensagem) = Facade.MotoService.Delete(id);
            return success ? Ok() : BadRequest(new { mensagem });
        }
    }
}
