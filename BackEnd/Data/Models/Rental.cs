using Data.Models.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Rental : BaseModel
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string MotorcycleId { get; set; }
        public required int RentalPlanId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required DateTime ExpectedEndDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public RentalPlan? RentalPlan { get; set; }
    }
}
