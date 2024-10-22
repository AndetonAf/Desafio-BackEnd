using BackEnd.DTOs;
using BackEnd.DTOs.LocacaoController;
using BackEnd.DTOs.MotosController;
using BackEnd.Services.Interfaces;
using BackEnd.Validators;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BackEnd.Services.RentalService;
using static Dapper.SqlMapper;

namespace BackEnd.Controllers.v1
{
    [Auth(UserType.DeliveryMan)]
    public class LocacaoController(IFacadeService facadeService) : AbstractV1Controller(facadeService)
    {

        [SwaggerOperation(Summary = "Consultar locação por id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RentalGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MensagemDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(MensagemDto))]
        [HttpGet("{id}")]
        public ActionResult GetById(string id)
        {
            (bool success, RentalGetDto? result) = Facade.RentalService.GetById(id);
            return success ? Ok(result) : NotFound(new MensagemDto("Locação não encontrada"));
        }


        [SwaggerOperation(Summary = "Informar data de devolução e calcular valor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RentalPutResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MensagemDto))]
        [HttpPut("{id}/devolucao")]
        public ActionResult ReturnDate(string id, [FromBody] RentalUpdateReturnDateDto dto)
        {
            (bool success, string mensagem, RentalValue? rentalValue) = Facade.RentalService.ReturnRental(id, dto.Data_devolucao);
            return success ? Ok(new RentalPutResponseDto(mensagem, rentalValue)) : BadRequest(mensagem);
        }

        [SwaggerOperation(Summary = "Alugar uma moto")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MensagemDto))]
        [HttpPost]
        public ActionResult Post(RentalDto dto)
        {
            (bool success, string mensagem) = Facade.RentalService.Create(dto);
            return success ? StatusCode(201) : BadRequest(new { mensagem });
        }
    }
}
