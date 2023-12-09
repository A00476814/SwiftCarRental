using SwiftCarRental.Areas.Identity.Data;

namespace SwiftCarRental.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int VehicleId { get; set; }
        public string LicenceNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public Vehicle Vehicle { get; set; }
        
        
    }
}
    