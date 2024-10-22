using Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd.DTOs.LocacaoController
{
    public class RentalGetDto
    {
        public required string Identificador { get; set; }
        public decimal Valor_diaria { get; set; }
        public required string Entregador_id { get; set; }
        public required string Moto_id { get; set; }
        public required DateTime Data_inicio { get; set; }
        public required DateTime Data_termino { get; set; }
        public required DateTime Data_previsao_termino { get; set; }
        public required DateTime? Data_devolucao { get; set; }

    }
}
