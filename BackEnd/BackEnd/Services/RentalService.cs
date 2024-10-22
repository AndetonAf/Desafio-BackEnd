using BackEnd.DTOs;
using BackEnd.DTOs.EntregadoresController;
using BackEnd.DTOs.LocacaoController;
using BackEnd.Services.Interfaces;
using Dapper;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BackEnd.Services
{
    public class RentalService(Context context) : IRentalService
    {
        readonly Context Context = context;

        public (bool success, string mensagem) Create(RentalDto dto)
        {
            if (Context.Rentals.Any(x => x.Id == dto.Identificador))
            {
                return (false, "Já cadastrado");
            }

            var user = Context.Users.FirstOrDefault(x => x.Id == dto.Entregador_id);
            if (user == null)
            {
                return (false, "Dados inválidos");
            }
            if(user.CnhType == CnhType.B)
            {
                return (false, "Motorista não tem a CNH A");
            }
            if (!Context.Motorcycles.Any(x => x.Id == dto.Moto_id))
            {
                return (false, "Dados inválidos");
            }
            var rentalPlans = Context.RentalPlans.FirstOrDefault(x => x.Days == dto.Plano);
            if (rentalPlans == null)
            {
                return (false, "Dados inválidos");
            }

            var startDate = DateTime.UtcNow.Date.AddDays(1);
            var rental = new Rental()
            {
                Id = dto.Identificador,
                UserId = dto.Entregador_id,
                MotorcycleId = dto.Moto_id,
                RentalPlanId = dto.Plano,
                StartDate = startDate,
                EndDate = startDate.AddDays(rentalPlans.Days).AddDays(1).AddTicks(-1),
                ExpectedEndDate = dto.Data_previsao_termino
            };
            Context.Rentals.Add(rental);
            Context.SaveChanges();

            return (true, "");
        }

        public (bool success, RentalGetDto? result) GetById(string id)
        {
            var rental = Context.Rentals.Include(x => x.RentalPlan).FirstOrDefault(x => x.Id == id);
            if (rental == null || rental.RentalPlan == null)
            {
                return (false, null);
            }

            return (true, new RentalGetDto
            {
                Identificador = rental.Id,
                Valor_diaria = rental.RentalPlan.Coust,
                Entregador_id = rental.UserId,
                Moto_id = rental.MotorcycleId,
                Data_inicio = rental.StartDate,
                Data_termino = rental.EndDate,
                Data_previsao_termino = rental.ExpectedEndDate,
                Data_devolucao = rental.ReturnDate
            });
        }

        public (bool success, string mensagem, RentalValue? rentalValue) ReturnRental(string id, DateTime returnDate)
        {
            var rental = Context.Rentals.Include(x => x.RentalPlan).FirstOrDefault(x => x.Id == id);
            if (rental == null || rental.RentalPlan == null)
            {
                return (false, "Dados inválidos", null);
            }

            rental.ReturnDate = returnDate;
            Context.Rentals.Update(rental);
            Context.SaveChanges();

            return (true, "Data de devolução informada com sucesso", GetValue(rental, rental.RentalPlan));
        }

        public record RentalValue(decimal Days, decimal ValueForDay, decimal SubTotal, decimal LateFee, decimal Total);
        public static RentalValue GetValue(Rental rental, RentalPlan rentalPlan)
        {
            if (rental.ReturnDate!.Value.Date < rental.ExpectedEndDate.Date)
                return CalculateEarlyReturn(rental, rentalPlan);

            if (rental.ReturnDate!.Value.Date > rental.ExpectedEndDate.Date)
                return CalculateLateReturn(rental, rentalPlan);

            return CalculateOnTimeReturn(rentalPlan);
        }

        private static RentalValue CalculateEarlyReturn(Rental rental, RentalPlan rentalPlan)
        {
            int daysNotPay = (rental.ExpectedEndDate.Date - rental.ReturnDate!.Value.Date).Days;
            int daysForPay = rentalPlan.Days - daysNotPay;
            decimal subTotal = ((rentalPlan.Days - daysNotPay) * rentalPlan.Coust);
            decimal lateFee = (daysNotPay * rentalPlan.Coust) * (rentalPlan.LateFeePercentage * 0.01m);
            decimal total = (daysForPay * rentalPlan.Coust) + lateFee;

            return new RentalValue(daysForPay, rentalPlan.Coust, subTotal, lateFee, total);
        }

        private static RentalValue CalculateLateReturn(Rental rental, RentalPlan rentalPlan)
        {
            int daysLate = (rental.ReturnDate!.Value.Date - rental.ExpectedEndDate.Date).Days;
            decimal lateFee = daysLate * 50;
            decimal total = (rentalPlan.Days * rentalPlan.Coust) + lateFee;

            return new RentalValue(rentalPlan.Days + daysLate, rentalPlan.Coust, rentalPlan.Days * rentalPlan.Coust, lateFee, total);
        }

        private static RentalValue CalculateOnTimeReturn(RentalPlan rentalPlan)
        {
            decimal total = rentalPlan.Days * rentalPlan.Coust;
            return new RentalValue(rentalPlan.Days, rentalPlan.Coust, total, 0, total);
        }
    }
}
