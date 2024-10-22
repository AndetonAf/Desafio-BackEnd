using BackEnd.DTOs;
using BackEnd.DTOs.EntregadoresController;
using BackEnd.Services.Interfaces;
using Dapper;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BackEnd.Services
{
    public class UserService(Context context) : IUserService
    {
        readonly Context Context = context;

        public bool Exist(string id)
        {
            return Context.Users.Any(x => x.Id == id);
        }

        public bool ExistCnhCnpj(string cnh, string cnpj)
        {
            return Context.Users.Where(x => x.CnhNumber == cnh || x.Cnpj == cnpj).Any();
        }

        public void Create(UserType userType, string id, UserDto dto, string pathCnh)
        {
            var user = new User()
            {
                Type = userType,
                Id = id,
                Name = dto.Nome,
                Cnpj = dto.Cnpj,
                DateBirth = dto.Data_nascimento,
                CnhNumber = dto.Numero_cnh,
                CnhType = dto.Tipo_cnh,
                CnhImage = pathCnh
            };

            Context.Users.Add(user);
            Context.SaveChanges();
        }

        public (bool success, string mensagem) UpdateCnhImage(string id, string pathCnh)
        {
            var user = Context.Users.First(x => x.Id == id);
            user.CnhImage = pathCnh;
            Context.Update(user);
            Context.SaveChanges();

            return (true, "");
        }
    }
}
