using BackEnd.DTOs;
using BackEnd.DTOs.EntregadoresController;
using Data.Models;

namespace BackEnd.Services.Interfaces
{
    public interface IUserService
    {
        public bool ExistCnhCnpj(string cnh, string cnpj);
        public bool Exist(string id);
        public void Create(UserType userType, string id, UserDto dto, string pathCnh);
        public (bool success, string mensagem) UpdateCnhImage(string id, string pathCnh);
    }
}
