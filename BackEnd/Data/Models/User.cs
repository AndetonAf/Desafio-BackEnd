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
    public enum UserType
    {
        Administrator = 1,
        DeliveryMan = 2
    }

    public enum CnhType
    {
        A = 1,
        B = 2,
        AB = 3
    }

    public class User : BaseModel
    {
        public UserType Type { get; set; }
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Cnpj { get; set; }
        public required DateTime DateBirth { get; set; }
        public required string CnhNumber { get; set; }
        public required CnhType CnhType { get; set; }
        public required string CnhImage { get; set; }

        public ICollection<Rental> Rentals { get; set; } = [];
    }
}
