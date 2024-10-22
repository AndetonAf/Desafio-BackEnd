using Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd.DTOs.EntregadoresController
{
    public class UserDto
    {
        [Required(ErrorMessage = "Preencha o Identificador")]
        public required string Identificador { get; set; }

        [Required(ErrorMessage = "Preencha o Nome")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o Cnpj")]
        public required string Cnpj { get; set; }

        [Required(ErrorMessage = "Preencha a Data de nascimento")]
        public required DateTime Data_nascimento { get; set; }

        [Required(ErrorMessage = "Preencha o numero da cnh")]
        public required string Numero_cnh { get; set; }

        [Required(ErrorMessage = "Preencha o tipo de cnh")]
        public required CnhType Tipo_cnh { get; set; }

        [Required(ErrorMessage = "Preencha o imagem da cnh")]
        [Base64String(ErrorMessage = "Imagem da cnh invalida")]
        public required string Imagem_cnh { get; set; }
    }
}
