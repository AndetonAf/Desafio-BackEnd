using BackEnd.DTOs;
using BackEnd.DTOs.MotosController;

namespace BackEnd.Services.Interfaces
{
    public interface IMotorcycleService
    {
        public MotorcycleDto[] GetAll(string? placa);
        public MotorcycleDto? GetById(string identificador);
        public (bool success, string mensagem) Create(MotorcycleDto dto);
        public (bool success, string mensagem) UpdatePlate(string id, string plate);
        public (bool success, string mensagem) Delete(string id);
    }
}
