using Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd.DTOs.MotosController
{
    public class UpdatePlateDto
    {
        public string Placa { get; set; } = "";
    }
}
