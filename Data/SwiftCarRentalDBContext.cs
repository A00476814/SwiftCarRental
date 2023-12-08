using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SwiftCarRental.Models;

namespace SwiftCarRental.Data
{
    public class SwiftCarRentalDBContext : DbContext
    {
        public SwiftCarRentalDBContext (DbContextOptions<SwiftCarRentalDBContext> options)
            : base(options)
        {
        }
        public DbSet<SwiftCarRental.Models.Vehicle> Vehicle { get; set; } = default!;
        public DbSet<SwiftCarRental.Models.Booking> Booking { get; set; } = default!;
        public DbSet<SwiftCarRental.Models.PaymentDetails> PaymentDetails { get; set; } = default!;


    }
}
