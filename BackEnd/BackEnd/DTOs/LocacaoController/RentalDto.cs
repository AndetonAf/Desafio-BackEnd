using Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd.DTOs.LocacaoController
{
    public class RentalDto
    {
        public required string Identificador { get; set; }

        [Required(ErrorMessage = "Preencha o Id do entregador")]
        public required string Entregador_id { get; set; }

        [Required(ErrorMessage = "Preencha o Id da moto")]
        public required string Moto_id { get; set; }

        [Required(ErrorMessage = "Preencha a data de inicio")]
        public required DateTime Data_inicio { get; set; }

        [Required(ErrorMessage = "Preencha a data de termino")]
        public required DateTime Data_termino { get; set; }

        [Required(ErrorMessage = "Preencha a data de previsao de termino")]
        public required DateTime Data_previsao_termino { get; set; }

        [Required(ErrorMessage = "Preencha o plano")]
        public required int Plano { get; set; }

    }
}
