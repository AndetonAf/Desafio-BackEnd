using BackEnd.DTOs;
using BackEnd.DTOs.EntregadoresController;
using BackEnd.DTOs.LocacaoController;
using Data.Models;
using static BackEnd.Services.RentalService;

namespace BackEnd.Services.Interfaces
{
    public interface IRentalService
    {
        public (bool success, RentalGetDto? result) GetById(string id);
        public (bool success, string mensagem) Create(RentalDto dto);
        public (bool success, string mensagem, RentalValue? rentalValue) ReturnRental(string id, DateTime returnDate);
    }
}
