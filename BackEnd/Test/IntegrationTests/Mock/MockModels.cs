using AutoFixture;
using BackEnd.DTOs.EntregadoresController;
using Data.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.IntegrationTests.Mock
{
    public static class MockModels
    {
        public static User User()
        {
            Fixture fixture = new();
            var user = fixture.Build<User>().Without(x => x.Rentals).Create();
            user.DateBirth = DateTime.UtcNow;
            return user;
        }

        public static IEnumerable<UserDto> UserDto(int count = 1)
        {
            Fixture fixture = new();
            var users = fixture.Build<UserDto>().CreateMany(count);
            foreach (var item in users)
            {
                item.Data_nascimento = DateTime.UtcNow;
            }
            return users;
        }

        public static IEnumerable<Motorcycle> Motorcycle(int count = 1)
        {
            Fixture fixture = new();
            var motorcycles = fixture.Build<Motorcycle>().Without(x => x.Rentals).CreateMany(count);
            return motorcycles;
        }

        public static Rental Rental(User user, Motorcycle motorcycle, int rentalPlanId = 7)
        {
            Fixture fixture = new();
            var rental = fixture.Build<Rental>().Without(x => x.RentalPlan).Create();
            rental.RentalPlanId = rentalPlanId;

            rental.StartDate = DateTime.UtcNow.AddDays(1).Date;
            rental.EndDate = DateTime.UtcNow.Date.AddDays(rental.RentalPlanId).AddDays(1).AddTicks(-1);
            rental.ExpectedEndDate = DateTime.UtcNow.AddDays(rental.RentalPlanId).AddDays(1).AddTicks(-1);
            rental.ReturnDate = null;

            rental.UserId = user.Id;
            rental.MotorcycleId = motorcycle.Id;

            return rental;
        }
    }
}
