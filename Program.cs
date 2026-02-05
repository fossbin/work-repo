using GoWheelsConsole.Data;
using GoWheelsConsole.Models;

namespace GoWheelsConsole;

class Program
{
    private static readonly UserRepository _userRepo = new();
    private static readonly VehicleRepository _vehicleRepo = new();
    private static readonly BookingRepository _bookingRepo = new();
    private static readonly AdminRepository _adminRepo = new();

    static void Main(string[] args)
    {
        Console.Title = "GoWheels - Vehicle Rental Management System";
        
        // Initialize database
        DatabaseHelper.InitializeDatabase();

        while (true)
        {
            ShowMainMenu();
            var choice = GetUserInput("Enter your choice: ");

            switch (choice)
            {
                case "1":
                    VehiclesMenu();
                    break;
                case "2":
                    UsersMenu();
                    break;
                case "3":
                    AdminsMenu();
                    break;
                case "4":
                    BookingsMenu();
                    break;
                case "5":
                    Console.WriteLine("\nThank you for using GoWheels! Goodbye.");
                    return;
                default:
                    Console.WriteLine("\nInvalid choice. Please try again.");
                    break;
            }
        }
    }

    static void ShowMainMenu()
    {
        SafeClear();
        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║     GOWHEELS - Vehicle Rental Management     ║");
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine("║  1. Vehicles Management                      ║");
        Console.WriteLine("║  2. Users Management                         ║");
        Console.WriteLine("║  3. Admins Management                        ║");
        Console.WriteLine("║  4. Bookings Management                      ║");
        Console.WriteLine("║  5. Exit                                     ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝");
    }

    #region Vehicles Menu

    static void VehiclesMenu()
    {
        while (true)
        {
            SafeClear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║           VEHICLES MANAGEMENT                ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. List All Vehicles                        ║");
            Console.WriteLine("║  2. Add New Vehicle                          ║");
            Console.WriteLine("║  3. Edit Vehicle                             ║");
            Console.WriteLine("║  4. Delete Vehicle                           ║");
            Console.WriteLine("║  5. Back to Main Menu                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            var choice = GetUserInput("Enter your choice: ");

            switch (choice)
            {
                case "1":
                    ListVehicles();
                    break;
                case "2":
                    AddVehicle();
                    break;
                case "3":
                    EditVehicle();
                    break;
                case "4":
                    DeleteVehicle();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Pause();
        }
    }

    static void ListVehicles()
    {
        SafeClear();
        Console.WriteLine("\n═══ ALL VEHICLES ═══\n");
        var vehicles = _vehicleRepo.GetAll();
        if (vehicles.Count == 0)
        {
            Console.WriteLine("No vehicles found.");
            return;
        }
        foreach (var vehicle in vehicles)
        {
            Console.WriteLine(vehicle);
        }
    }

    static void AddVehicle()
    {
        SafeClear();
        Console.WriteLine("\n═══ ADD NEW VEHICLE ═══\n");

        var vehicle = new Vehicle
        {
            Name = GetUserInput("Vehicle Name (e.g., Honda City): "),
            Type = GetVehicleType(),
            PricePerDay = GetDecimalInput("Price Per Day (₹): "),
            VehicleNumber = GetUserInput("Vehicle Number (e.g., KA01AB1234): "),
            Status = GetVehicleStatus(),
            Description = GetUserInput("Description (optional): ")
        };

        try
        {
            _vehicleRepo.Add(vehicle);
            Console.WriteLine("\n✓ Vehicle added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error adding vehicle: {ex.Message}");
        }
    }

    static void EditVehicle()
    {
        SafeClear();
        ListVehicles();
        Console.WriteLine();

        var id = GetIntInput("Enter Vehicle ID to edit: ");
        var vehicle = _vehicleRepo.GetById(id);

        if (vehicle == null)
        {
            Console.WriteLine("Vehicle not found.");
            return;
        }

        Console.WriteLine($"\nEditing: {vehicle.Name}");
        Console.WriteLine("(Press Enter to keep current value)\n");

        var name = GetUserInput($"Vehicle Name [{vehicle.Name}]: ");
        if (!string.IsNullOrWhiteSpace(name)) vehicle.Name = name;

        Console.WriteLine($"Current Type: {vehicle.Type}");
        var changeType = GetUserInput("Change type? (y/n): ");
        if (changeType.ToLower() == "y") vehicle.Type = GetVehicleType();

        var priceStr = GetUserInput($"Price Per Day [{vehicle.PricePerDay}]: ");
        if (!string.IsNullOrWhiteSpace(priceStr) && decimal.TryParse(priceStr, out var price))
            vehicle.PricePerDay = price;

        var number = GetUserInput($"Vehicle Number [{vehicle.VehicleNumber}]: ");
        if (!string.IsNullOrWhiteSpace(number)) vehicle.VehicleNumber = number;

        Console.WriteLine($"Current Status: {vehicle.Status}");
        var changeStatus = GetUserInput("Change status? (y/n): ");
        if (changeStatus.ToLower() == "y") vehicle.Status = GetVehicleStatus();

        var desc = GetUserInput($"Description [{vehicle.Description}]: ");
        if (!string.IsNullOrWhiteSpace(desc)) vehicle.Description = desc;

        try
        {
            _vehicleRepo.Update(vehicle);
            Console.WriteLine("\n✓ Vehicle updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error updating vehicle: {ex.Message}");
        }
    }

    static void DeleteVehicle()
    {
        SafeClear();
        ListVehicles();
        Console.WriteLine();

        var id = GetIntInput("Enter Vehicle ID to delete: ");
        var vehicle = _vehicleRepo.GetById(id);

        if (vehicle == null)
        {
            Console.WriteLine("Vehicle not found.");
            return;
        }

        var confirm = GetUserInput($"Are you sure you want to delete '{vehicle.Name}'? (y/n): ");
        if (confirm.ToLower() == "y")
        {
            try
            {
                _vehicleRepo.Delete(id);
                Console.WriteLine("\n✓ Vehicle deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error deleting vehicle: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
    }

    static string GetVehicleType()
    {
        Console.WriteLine("Select Vehicle Type:");
        Console.WriteLine("  1. Car");
        Console.WriteLine("  2. Bike");
        Console.WriteLine("  3. Scooter");
        Console.WriteLine("  4. SUV");
        var choice = GetUserInput("Enter choice: ");
        return choice switch
        {
            "1" => "Car",
            "2" => "Bike",
            "3" => "Scooter",
            "4" => "SUV",
            _ => "Car"
        };
    }

    static string GetVehicleStatus()
    {
        Console.WriteLine("Select Status:");
        Console.WriteLine("  1. Available");
        Console.WriteLine("  2. Lent");
        Console.WriteLine("  3. Disabled");
        var choice = GetUserInput("Enter choice: ");
        return choice switch
        {
            "1" => "Available",
            "2" => "Lent",
            "3" => "Disabled",
            _ => "Available"
        };
    }

    #endregion

    #region Users Menu

    static void UsersMenu()
    {
        while (true)
        {
            SafeClear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║             USERS MANAGEMENT                 ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. List All Users                           ║");
            Console.WriteLine("║  2. Add New User                             ║");
            Console.WriteLine("║  3. Edit User                                ║");
            Console.WriteLine("║  4. Delete User                              ║");
            Console.WriteLine("║  5. Back to Main Menu                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            var choice = GetUserInput("Enter your choice: ");

            switch (choice)
            {
                case "1":
                    ListUsers();
                    break;
                case "2":
                    AddUser();
                    break;
                case "3":
                    EditUser();
                    break;
                case "4":
                    DeleteUser();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Pause();
        }
    }

    static void ListUsers()
    {
        SafeClear();
        Console.WriteLine("\n═══ ALL USERS ═══\n");
        var users = _userRepo.GetAll();
        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }
        foreach (var user in users)
        {
            Console.WriteLine(user);
        }
    }

    static void AddUser()
    {
        SafeClear();
        Console.WriteLine("\n═══ ADD NEW USER ═══\n");

        var user = new User
        {
            FirstName = GetUserInput("First Name: "),
            LastName = GetUserInput("Last Name: "),
            Email = GetUserInput("Email: "),
            Password = GetUserInput("Password: ")
        };

        try
        {
            _userRepo.Add(user);
            Console.WriteLine("\n✓ User added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error adding user: {ex.Message}");
        }
    }

    static void EditUser()
    {
        SafeClear();
        ListUsers();
        Console.WriteLine();

        var id = GetIntInput("Enter User ID to edit: ");
        var user = _userRepo.GetById(id);

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.WriteLine($"\nEditing: {user.FirstName} {user.LastName}");
        Console.WriteLine("(Press Enter to keep current value)\n");

        var firstName = GetUserInput($"First Name [{user.FirstName}]: ");
        if (!string.IsNullOrWhiteSpace(firstName)) user.FirstName = firstName;

        var lastName = GetUserInput($"Last Name [{user.LastName}]: ");
        if (!string.IsNullOrWhiteSpace(lastName)) user.LastName = lastName;

        var email = GetUserInput($"Email [{user.Email}]: ");
        if (!string.IsNullOrWhiteSpace(email)) user.Email = email;

        var password = GetUserInput($"Password [hidden]: ");
        if (!string.IsNullOrWhiteSpace(password)) user.Password = password;

        try
        {
            _userRepo.Update(user);
            Console.WriteLine("\n✓ User updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error updating user: {ex.Message}");
        }
    }

    static void DeleteUser()
    {
        SafeClear();
        ListUsers();
        Console.WriteLine();

        var id = GetIntInput("Enter User ID to delete: ");
        var user = _userRepo.GetById(id);

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        var confirm = GetUserInput($"Are you sure you want to delete '{user.FirstName} {user.LastName}'? (y/n): ");
        if (confirm.ToLower() == "y")
        {
            try
            {
                _userRepo.Delete(id);
                Console.WriteLine("\n✓ User deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error deleting user: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
    }


    #endregion

    #region Admins Menu

    static void AdminsMenu()
    {
        while (true)
        {
            SafeClear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║            ADMINS MANAGEMENT                 ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. List All Admins                          ║");
            Console.WriteLine("║  2. Add New Admin                            ║");
            Console.WriteLine("║  3. Edit Admin                               ║");
            Console.WriteLine("║  4. Delete Admin                             ║");
            Console.WriteLine("║  5. Back to Main Menu                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            var choice = GetUserInput("Enter your choice: ");

            switch (choice)
            {
                case "1":
                    ListAdmins();
                    break;
                case "2":
                    AddAdmin();
                    break;
                case "3":
                    EditAdmin();
                    break;
                case "4":
                    DeleteAdmin();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Pause();
        }
    }

    static void ListAdmins()
    {
        SafeClear();
        Console.WriteLine("\n═══ ALL ADMINS ═══\n");
        var admins = _adminRepo.GetAll();
        if (admins.Count == 0)
        {
            Console.WriteLine("No admins found.");
            return;
        }
        foreach (var admin in admins)
        {
            Console.WriteLine(admin);
        }
    }

    static void AddAdmin()
    {
        SafeClear();
        Console.WriteLine("\n═══ ADD NEW ADMIN ═══\n");

        var admin = new Admin
        {
            Username = GetUserInput("Username: "),
            Email = GetUserInput("Email: "),
            Password = GetUserInput("Password: ")
        };

        try
        {
            _adminRepo.Add(admin);
            Console.WriteLine("\n✓ Admin added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error adding admin: {ex.Message}");
        }
    }

    static void EditAdmin()
    {
        SafeClear();
        ListAdmins();
        Console.WriteLine();

        var id = GetIntInput("Enter Admin ID to edit: ");
        var admin = _adminRepo.GetById(id);

        if (admin == null)
        {
            Console.WriteLine("Admin not found.");
            return;
        }

        Console.WriteLine($"\nEditing: {admin.Username}");
        Console.WriteLine("(Press Enter to keep current value)\n");

        var username = GetUserInput($"Username [{admin.Username}]: ");
        if (!string.IsNullOrWhiteSpace(username)) admin.Username = username;

        var email = GetUserInput($"Email [{admin.Email}]: ");
        if (!string.IsNullOrWhiteSpace(email)) admin.Email = email;

        var password = GetUserInput($"Password [hidden]: ");
        if (!string.IsNullOrWhiteSpace(password)) admin.Password = password;

        try
        {
            _adminRepo.Update(admin);
            Console.WriteLine("\n✓ Admin updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error updating admin: {ex.Message}");
        }
    }

    static void DeleteAdmin()
    {
        SafeClear();
        ListAdmins();
        Console.WriteLine();

        var id = GetIntInput("Enter Admin ID to delete: ");
        var admin = _adminRepo.GetById(id);

        if (admin == null)
        {
            Console.WriteLine("Admin not found.");
            return;
        }

        var confirm = GetUserInput($"Are you sure you want to delete admin '{admin.Username}'? (y/n): ");
        if (confirm.ToLower() == "y")
        {
            try
            {
                _adminRepo.Delete(id);
                Console.WriteLine("\n✓ Admin deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error deleting admin: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }
    }

    #endregion

    #region Bookings Menu

    static void BookingsMenu()
    {
        while (true)
        {
            SafeClear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║           BOOKINGS MANAGEMENT                ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. List All Bookings                        ║");
            Console.WriteLine("║  2. Create New Booking                       ║");
            Console.WriteLine("║  3. Cancel Booking                           ║");
            Console.WriteLine("║  4. Back to Main Menu                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            var choice = GetUserInput("Enter your choice: ");

            switch (choice)
            {
                case "1":
                    ListBookings();
                    break;
                case "2":
                    CreateBooking();
                    break;
                case "3":
                    CancelBooking();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Pause();
        }
    }

    static void ListBookings()
    {
        Console.Clear();
        Console.WriteLine("\n═══ ALL BOOKINGS ═══\n");
        var bookings = _bookingRepo.GetAll();
        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings found.");
            return;
        }
        foreach (var booking in bookings)
        {
            Console.WriteLine(booking);
        }
    }

    static void CreateBooking()
    {
        Console.Clear();
        Console.WriteLine("\n═══ CREATE NEW BOOKING ═══\n");

        // Show available vehicles
        Console.WriteLine("Available Vehicles:");
        var vehicles = _vehicleRepo.GetAvailable();
        if (vehicles.Count == 0)
        {
            Console.WriteLine("No vehicles available for booking.");
            return;
        }
        foreach (var v in vehicles)
        {
            Console.WriteLine($"  {v}");
        }
        Console.WriteLine();

        var vehicleId = GetIntInput("Enter Vehicle ID to book: ");
        var vehicle = _vehicleRepo.GetById(vehicleId);
        if (vehicle == null || vehicle.Status != "Available")
        {
            Console.WriteLine("Invalid vehicle or not available.");
            return;
        }

        // Show users
        Console.WriteLine("\nUsers:");
        var users = _userRepo.GetAll();
        foreach (var u in users)
        {
            Console.WriteLine($"  {u}");
        }
        Console.WriteLine();

        var userId = GetIntInput("Enter User ID: ");
        if (_userRepo.GetById(userId) == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        var startDate = GetDateInput("Start Date (yyyy-mm-dd): ");
        var endDate = GetDateInput("End Date (yyyy-mm-dd): ");

        if (endDate < startDate)
        {
            Console.WriteLine("End date cannot be before start date.");
            return;
        }

        var pickupLocation = GetUserInput("Pickup Location: ");

        var totalDays = (endDate - startDate).Days + 1;
        var rentalCost = totalDays * vehicle.PricePerDay;
        var securityDeposit = 500m;
        var totalCost = rentalCost + securityDeposit;

        Console.WriteLine($"\n═══ BOOKING SUMMARY ═══");
        Console.WriteLine($"Vehicle: {vehicle.Name}");
        Console.WriteLine($"Duration: {totalDays} days");
        Console.WriteLine($"Rental Cost: ₹{rentalCost}");
        Console.WriteLine($"Security Deposit: ₹{securityDeposit}");
        Console.WriteLine($"Total: ₹{totalCost}");

        var confirm = GetUserInput("\nConfirm booking? (y/n): ");
        if (confirm.ToLower() == "y")
        {
            var booking = new Booking
            {
                UserId = userId,
                VehicleId = vehicleId,
                StartDate = startDate,
                EndDate = endDate,
                PickupLocation = pickupLocation,
                TotalCost = totalCost
            };

            try
            {
                _bookingRepo.Add(booking);
                _vehicleRepo.UpdateStatus(vehicleId, "Lent");
                Console.WriteLine("\n✓ Booking created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error creating booking: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Booking cancelled.");
        }
    }

    static void CancelBooking()
    {
        Console.Clear();
        ListBookings();
        Console.WriteLine();

        var id = GetIntInput("Enter Booking ID to cancel: ");
        var booking = _bookingRepo.GetById(id);

        if (booking == null)
        {
            Console.WriteLine("Booking not found.");
            return;
        }

        var confirm = GetUserInput($"Are you sure you want to cancel booking #{id}? (y/n): ");
        if (confirm.ToLower() == "y")
        {
            try
            {
                _bookingRepo.Delete(id);
                _vehicleRepo.UpdateStatus(booking.VehicleId, "Available");
                Console.WriteLine("\n✓ Booking cancelled successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error cancelling booking: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Cancellation aborted.");
        }
    }

    #endregion

    #region Helper Methods

    static string GetUserInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    static int GetIntInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var result))
                return result;
            Console.WriteLine("Please enter a valid number.");
        }
    }

    static decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out var result))
                return result;
            Console.WriteLine("Please enter a valid amount.");
        }
    }

    static DateTime GetDateInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (DateTime.TryParse(Console.ReadLine(), out var result))
                return result;
            Console.WriteLine("Please enter a valid date (yyyy-mm-dd).");
        }
    }

    static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        try { Console.ReadKey(); } catch { }
    }

    static void SafeClear()
    {
        try
        {
            Console.Clear();
        }
        catch
        {
            // If Console.Clear() fails (e.g., in non-interactive terminal), just print newlines
            Console.WriteLine("\n\n");
        }
    }

    #endregion
}
