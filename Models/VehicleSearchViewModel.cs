namespace SwiftCarRental.Models
{
    public class VehicleSearchViewModel
    {
        public List<Vehicle> AvailableVehicles { get; set; }

        public int numberOfHours { get; set; } = 1;

        public DateTime fromDate { get; set; } 

        public DateTime toDate { get; set; }
        public BookingDetails BookingDetails { get; set; }

        public VehicleSearchViewModel()
        {
            AvailableVehicles = new List<Vehicle>();
        }
    }
}
