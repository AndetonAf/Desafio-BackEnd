using Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd.DTOs.MotosController
{
    public class MotorcycleDto
    {
        public required string Identificador { get; set; }

        [Range(1900, Int32.MaxValue, ErrorMessage = "Ano Inválido")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "Preencha o Modelo")]
        public required string Modelo { get; set; }

        [Required(ErrorMessage = "Preencha a Placa")]
        public required string Placa { get; set; }

        public static explicit operator MotorcycleDto(Motorcycle model)
        {
            return new MotorcycleDto
            {
                Identificador = model.Id,
                Ano = model.Year,
                Modelo = model.Model,
                Placa = model.Plate
            };
        }
    }
}
