namespace GoWheelsConsole.Models;

public class Vehicle
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
    public decimal PricePerDay { get; set; }
    public string VehicleNumber { get; set; } = "";
    public string Status { get; set; } = "Available";
    public string Description { get; set; } = "";
}
