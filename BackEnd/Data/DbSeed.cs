using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class DbSeed
    {
        public static void Run(ModelBuilder builder)
        {
            Users(builder);
            RentalPlans(builder);
        }

        private static void RentalPlans(ModelBuilder builder)
        {
            RentalPlan[] rentalPlans = [
                new RentalPlan() { Days = 7, Coust = 30, LateFeePercentage = 20 },
                new RentalPlan() { Days = 15, Coust = 28, LateFeePercentage = 40 },
                new RentalPlan() { Days = 30, Coust = 22, LateFeePercentage = 40 },
                new RentalPlan() { Days = 45, Coust = 20, LateFeePercentage = 40 },
                new RentalPlan() { Days = 50, Coust = 18, LateFeePercentage = 40 }
            ];
            builder.Entity<RentalPlan>().HasData(rentalPlans);
        }

        public static void Users(ModelBuilder builder)
        {
            var userAdmin = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin User",
                Cnpj = "",
                DateBirth = DateTime.UtcNow,
                CnhNumber = "",
                CnhType = CnhType.AB,
                CnhImage = "",
                Type = UserType.Administrator
            };

            var userDelivery = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Delivery User",
                Cnpj = "",
                DateBirth = DateTime.UtcNow,
                CnhNumber = "",
                CnhType = CnhType.AB,
                CnhImage = "",
                Type = UserType.DeliveryMan
            };

            builder.Entity<User>().HasData(userAdmin, userDelivery);
        }
    }
}
