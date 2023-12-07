namespace SwiftCarRental.Models
{
    public class VehicleSearchViewModel
    {
        public List<Vehicle> AvailableVehicles { get; set; }

        public int numberOfHours { get; set; } = 1; 


        public VehicleSearchViewModel()
        {
            AvailableVehicles = new List<Vehicle>();
        }
    }
}
