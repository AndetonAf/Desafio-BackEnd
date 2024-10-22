using AutoFixture;
using BackEnd.Services;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BackEnd.Services.RentalService;

namespace Test.UnitTests
{
    public class RentalServiceTest
    {
        public static IEnumerable<object[]> GetRentalExamples()
        {
            var fixture = new Fixture().Build<Rental>().Without(x => x.RentalPlan);

            var rp7 = new RentalPlan() { Days = 7, Coust = 30, LateFeePercentage = 20 };
            var rp15 = new RentalPlan() { Days = 15, Coust = 28, LateFeePercentage = 40 };

            var startDate = DateTime.UtcNow.Date.AddDays(1);

            var rental = fixture.Create();
            rental.StartDate = startDate;
            rental.ExpectedEndDate = startDate.AddDays(rp7.Days).AddDays(1).AddTicks(-1);
            rental.ReturnDate = rental.ExpectedEndDate;
            yield return new object[] { rental, rp7, new RentalValue(7, 30, 210, 0, 210) };

            rental = fixture.Create();
            rental.StartDate = startDate;
            rental.ExpectedEndDate = startDate.AddDays(rp15.Days).AddDays(1).AddTicks(-1);
            rental.ReturnDate = rental.ExpectedEndDate;
            yield return new object[] { rental, rp15, new RentalValue(15, 28, 420, 0, 420) };

            rental = fixture.Create();
            rental.StartDate = startDate;
            rental.ExpectedEndDate = startDate.AddDays(rp7.Days).AddDays(1).AddTicks(-1);
            rental.ReturnDate = startDate.AddDays(10).AddDays(1).AddTicks(-1);
            yield return new object[] { rental, rp7, new RentalValue(10, 30, 210, 150, 360) };

            rental = fixture.Create();
            rental.StartDate = startDate;
            rental.ExpectedEndDate = startDate.AddDays(rp7.Days).AddDays(1).AddTicks(-1);
            rental.ReturnDate = startDate.AddDays(5).AddDays(1).AddTicks(-1);
            yield return new object[] { rental, rp7, new RentalValue(5, 30, 150, 12, 162) };
        }

        [Theory]
        [MemberData(nameof(GetRentalExamples))]
        public void GetValue(Rental rental, RentalPlan rentalPlan, RentalValue expected)
        {
            var result = RentalService.GetValue(rental, rentalPlan);
            Assert.Equal(expected.Days, result.Days);
            Assert.Equal(expected.ValueForDay, result.ValueForDay);
            Assert.Equal(expected.SubTotal, result.SubTotal);
            Assert.Equal(expected.LateFee, result.LateFee);
            Assert.Equal(expected.Total, result.Total);
        }

    }
}
