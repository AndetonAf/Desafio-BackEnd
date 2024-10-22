using BackEnd.DTOs;
using BackEnd.DTOs.MessagingService;
using BackEnd.DTOs.MotosController;
using BackEnd.Services.Interfaces;
using Dapper;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace BackEnd.Services
{
    public class MotorcycleService(Context context, IMessagingService messagingService) : IMotorcycleService
    {
        readonly Context Context = context;
        readonly IMessagingService _messagingService = messagingService;

        public MotorcycleDto[] GetAll(string? plate)
        {
            var query = new StringBuilder(@"SELECT ""Id"" as ""Identificador"", ""Year"" as ""Ano"", ""Model"" as ""Modelo"", ""Plate"" as ""Placa"" FROM ""Motorcycles"" WHERE ""Deleted"" is null");

            if (!string.IsNullOrEmpty(plate))
            {
                query.Append(@$" AND ""Plate"" = @plate ");
            }

            using var connection = Context.Database.GetDbConnection();
            connection.Open();

            return connection.Query<MotorcycleDto>(query.ToString(), new { plate }).ToArray();
        }

        public MotorcycleDto? GetById(string identificador)
        {
            var motorcycle = Context.Motorcycles.FirstOrDefault(x => x.Id == identificador);
            return motorcycle == null ? null : (MotorcycleDto)motorcycle;
        }

        public (bool success, string mensagem) Create(MotorcycleDto dto)
        {
            if (Context.Motorcycles.Any(x => x.Id == dto.Identificador))
            {
                return (false, "Já cadastrado");
            }

            var existPlace = Context.Motorcycles.Any(x => x.Plate == dto.Placa);
            if (existPlace)
            {
                return (false, "Placa já cadastrada");
            }

            var motorcycle = new Motorcycle()
            {
                Id = dto.Identificador,
                Year = dto.Ano,
                Model = dto.Modelo,
                Plate = dto.Placa
            };

            Context.Motorcycles.Add(motorcycle);
            Context.SaveChanges();

            var msg = new IdYearDto(motorcycle.Id.ToString(), motorcycle.Year);
            _messagingService.PublishMessagesAsync([JsonConvert.SerializeObject(msg)], "motorcycles").Wait();

            return (true, "");
        }

        public (bool success, string mensagem) UpdatePlate(string id, string plate)
        {
            var existPlace = Context.Motorcycles.Any(x => x.Id != id && x.Plate == plate);
            if (existPlace)
            {
                return (false, "Placa já cadastrada");
            }

            var motorcycle = Context.Motorcycles.FirstOrDefault(x => x.Id == id);
            if (motorcycle == null)
            {
                return (false, "Dados inválidos");
            }

            motorcycle.Plate = plate;
            Context.Update(motorcycle);
            Context.SaveChanges();

            return (true, "");
        }

        public (bool success, string mensagem) Delete(string id)
        {
            var motorcycle = Context.Motorcycles.Include(x => x.Rentals).FirstOrDefault(x => x.Id == id);
            if (motorcycle == null || motorcycle.Rentals.Any())
            {
                return (false, "Dados inválidos");
            }

            Context.Remove(motorcycle);
            Context.SaveChanges();

            return (true, "");
        }
    }
}
