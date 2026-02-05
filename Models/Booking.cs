namespace GoWheelsConsole.Models;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int VehicleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string PickupLocation { get; set; } = "";
    public decimal TotalCost { get; set; }
    public string UserName { get; set; } = "";
    public string VehicleName { get; set; } = "";
}
