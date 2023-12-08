using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftCarRental.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public string? Model { get; set; }
        public string Brand { get; set; }

        public int? ModelYear { get; set; }
        public string? Type { get; set; }
        public int? Odometer { get; set; }
        public int? NoOfSeats { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PricePerHour { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<DateTime> BookingFromDates { get; set; }
        public List<DateTime> BookingToDates { get; set; }

        public Vehicle()
        {
            Bookings = new List<Booking>();
            BookingFromDates = new List<DateTime>();
            BookingToDates = new List<DateTime>();
        }

    }
}
    