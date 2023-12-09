using SwiftCarRental.Areas.Identity.Data;

namespace SwiftCarRental.Models
{
    public class BookingDetails
    {
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public Vehicle Vehicle { get; set; }
        public SwiftUser User { get; set; }

    }
}
