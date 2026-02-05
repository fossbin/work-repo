namespace GoWheelsConsole.Models;

public class Vehicle
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Car, Bike, Scooter, SUV
    public decimal PricePerDay { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "Available"; // Available, Lent, Disabled
    public string? Description { get; set; }

    public override string ToString()
    {
        return $"[{Id}] {Name} ({Type}) | ₹{PricePerDay}/day | {VehicleNumber} | Status: {Status}";
    }
}
