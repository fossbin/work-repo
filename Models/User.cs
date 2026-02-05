namespace GoWheelsConsole.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "user"; // "admin" or "user"

    public override string ToString()
    {
        return $"[{Id}] {FirstName} {LastName} | {Email} | Role: {Role}";
    }
}
