namespace GoWheelsConsole.Models;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int VehicleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string PickupLocation { get; set; } = string.Empty;
    public decimal TotalCost { get; set; }

    // Navigation properties (for display purposes)
    public string? UserName { get; set; }
    public string? VehicleName { get; set; }

    public int TotalDays => (EndDate - StartDate).Days + 1;

    public override string ToString()
    {
        return $"[{Id}] {VehicleName ?? $"Vehicle #{VehicleId}"} | {UserName ?? $"User #{UserId}"} | {StartDate:dd MMM yyyy} to {EndDate:dd MMM yyyy} ({TotalDays} days) | ₹{TotalCost}";
    }
}
