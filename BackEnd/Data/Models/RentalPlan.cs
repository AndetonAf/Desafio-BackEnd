using Data.Models.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class RentalPlan : BaseModel
    {
        public int Days { get; set; }
        public decimal Coust { get; set; }
        public decimal LateFeePercentage { get; set; }

        public ICollection<Rental> Rentals { get; set; } = [];
    }
}
