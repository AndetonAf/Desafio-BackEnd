using Data.Models.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Motorcycle : BaseModel
    {
        public required string Id { get; set; }
        public int Year { get; set; }
        public required string Model { get; set; }
        public required string Plate { get; set; }

        public ICollection<Rental> Rentals { get; set; } = [];
    }
}
