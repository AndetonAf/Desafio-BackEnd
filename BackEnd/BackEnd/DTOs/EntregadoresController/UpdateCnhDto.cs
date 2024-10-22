using Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd.DTOs.EntregadoresController
{
    public class UpdateCnhDto
    {
        public string imagem_cnh { get; set; } = "";
    }
}
