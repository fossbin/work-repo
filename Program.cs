using GoWheelsConsole.Data;
using GoWheelsConsole.Models;

namespace GoWheelsConsole;

class Program
{
    static User currentUser = null;
    static Admin currentAdmin = null;

    static void Main(string[] args)
    {
        DatabaseHelper.InitializeDatabase();

        while (true)
        {
            Console.WriteLine("\n=== GOWHEELS MENU ===");
            Console.WriteLine("1. User Login");
            Console.WriteLine("2. User Registration");
            Console.WriteLine("3. Admin Login");
            Console.WriteLine("4. Exit");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            if (choice == "1") UserLogin();
            else if (choice == "2") UserRegistration();
            else if (choice == "3") AdminLogin();
            else if (choice == "4") break;
        }
    }

    // ========== USER REGISTRATION ==========
    static void UserRegistration()
    {
        Console.WriteLine("\n--- USER REGISTRATION ---");
        Console.Write("First Name: ");
        var firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        var lastName = Console.ReadLine();
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || 
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Error: All fields are required!");
            return;
        }

        var userRepo = new UserRepository();
        var existingUser = userRepo.GetByEmail(email);
        if (existingUser != null)
        {
            Console.WriteLine("Error: Email already exists!");
            return;
        }

        var user = new User();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.Password = password;
        userRepo.Add(user);
        Console.WriteLine("Registration successful!");
    }

    // ========== USER LOGIN ==========
    static void UserLogin()
    {
        Console.WriteLine("\n--- USER LOGIN ---");
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        var userRepo = new UserRepository();
        var user = userRepo.GetByEmail(email);
        if (user == null || user.Password != password)
        {
            Console.WriteLine("Invalid email or password!");
            return;
        }

        currentUser = user;
        Console.WriteLine($"Welcome, {user.FirstName}!");
        UserDashboard();
    }

    // ========== ADMIN LOGIN ==========
    static void AdminLogin()
    {
        Console.WriteLine("\n--- ADMIN LOGIN ---");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        var adminRepo = new AdminRepository();
        var admin = adminRepo.GetByUsername(username);
        if (admin == null || admin.Password != password)
        {
            Console.WriteLine("Invalid username or password!");
            return;
        }

        currentAdmin = admin;
        Console.WriteLine($"Welcome, {admin.Username}!");
        AdminDashboard();
    }

    // ========== USER DASHBOARD ==========
    static void UserDashboard()
    {
        while (currentUser != null)
        {
            Console.WriteLine("\n--- USER DASHBOARD ---");
            Console.WriteLine("1. Browse Vehicles");
            Console.WriteLine("2. My Bookings");
            Console.WriteLine("3. Create Booking");
            Console.WriteLine("4. Cancel Booking");
            Console.WriteLine("5. Logout");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            if (choice == "1") BrowseVehicles();
            else if (choice == "2") MyBookings();
            else if (choice == "3") CreateBooking();
            else if (choice == "4") CancelBooking();
            else if (choice == "5")
            {
                currentUser = null;
                Console.WriteLine("Logged out successfully!");
            }
        }
    }

    static void BrowseVehicles()
    {
        Console.WriteLine("\n--- AVAILABLE VEHICLES ---");
        var vehicleRepo = new VehicleRepository();
        var vehicles = vehicleRepo.GetAvailable();
        foreach (var v in vehicles)
            Console.WriteLine($"[{v.Id}] {v.Name} ({v.Type}) | Rs.{v.PricePerDay}/day | {v.VehicleNumber}");
    }

    static void MyBookings()
    {
        Console.WriteLine("\n--- MY BOOKINGS ---");
        var bookingRepo = new BookingRepository();
        var bookings = bookingRepo.GetByUserId(currentUser.Id);
        foreach (var b in bookings)
        {
            int days = (b.EndDate - b.StartDate).Days + 1;
            Console.WriteLine($"[{b.Id}] {b.VehicleName} | {b.StartDate:dd MMM} to {b.EndDate:dd MMM} ({days} days) | Rs.{b.TotalCost}");
        }
    }

    static void CreateBooking()
    {
        Console.WriteLine("\n--- CREATE BOOKING ---");
        Console.WriteLine("Available Vehicles:");
        var vehicleRepo = new VehicleRepository();
        var vehicles = vehicleRepo.GetAvailable();
        
        if (vehicles.Count == 0)
        {
            Console.WriteLine("No vehicles available at the moment!");
            return;
        }
        
        foreach (var v in vehicles)
            Console.WriteLine($"[{v.Id}] {v.Name} ({v.Type}) | Rs.{v.PricePerDay}/day");

        try
        {
            Console.Write("Vehicle ID: ");
            if (!int.TryParse(Console.ReadLine(), out int vehicleId))
            {
                Console.WriteLine("Error: Invalid vehicle ID!");
                return;
            }
            
            var vehicle = vehicleRepo.GetById(vehicleId);

            if (vehicle == null || vehicle.Status != "Available")
            {
                Console.WriteLine("Vehicle not available!");
                return;
            }

            Console.Write("Start Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                Console.WriteLine("Error: Invalid start date format!");
                return;
            }
            
            Console.Write("End Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                Console.WriteLine("Error: Invalid end date format!");
                return;
            }
            
            if (endDate <= startDate)
            {
                Console.WriteLine("Error: End date must be after start date!");
                return;
            }
            
            Console.Write("Pickup Location: ");
            string pickup = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(pickup))
            {
                Console.WriteLine("Error: Pickup location cannot be empty!");
                return;
            }

            int days = (endDate - startDate).Days + 1;
            decimal total = days * vehicle.PricePerDay + 500;

            var booking = new Booking();
            booking.UserId = currentUser.Id;
            booking.VehicleId = vehicleId;
            booking.StartDate = startDate;
            booking.EndDate = endDate;
            booking.PickupLocation = pickup;
            booking.TotalCost = total;

            var bookingRepo = new BookingRepository();
            bookingRepo.Add(booking);
            vehicleRepo.UpdateStatus(vehicleId, "Lent");
            Console.WriteLine($"Booking created. Total: Rs.{total}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating booking: {ex.Message}");
        }
    }

    static void CancelBooking()
    {
        Console.WriteLine("\n--- CANCEL BOOKING ---");
        Console.Write("Enter Booking ID: ");
        
        try
        {
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Error: Invalid booking ID!");
                return;
            }
            
            var bookingRepo = new BookingRepository();
            var booking = bookingRepo.GetById(id);
            
            if (booking == null)
            {
                Console.WriteLine("Error: Booking not found!");
                return;
            }

            if (booking.UserId != currentUser.Id)
            {
                Console.WriteLine("Error: You can only cancel your own bookings!");
                return;
            }

            bookingRepo.Delete(id);
            var vehicleRepo = new VehicleRepository();
            vehicleRepo.UpdateStatus(booking.VehicleId, "Available");
            Console.WriteLine("Booking cancelled successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cancelling booking: {ex.Message}");
        }
    }

    // ========== ADMIN DASHBOARD ==========
    static void AdminDashboard()
    {
        while (currentAdmin != null)
        {
            Console.WriteLine("\n--- ADMIN DASHBOARD ---");
            Console.WriteLine("1. Vehicles");
            Console.WriteLine("2. Users");
            Console.WriteLine("3. Admins");
            Console.WriteLine("4. Bookings");
            Console.WriteLine("5. Logout");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            if (choice == "1") VehiclesMenu();
            else if (choice == "2") UsersMenu();
            else if (choice == "3") AdminsMenu();
            else if (choice == "4") BookingsMenu();
            else if (choice == "5")
            {
                currentAdmin = null;
                Console.WriteLine("Logged out successfully!");
            }
        }
    }

    // ========== VEHICLES ==========
    static void VehiclesMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- VEHICLES ---");
            Console.WriteLine("1. List");
            Console.WriteLine("2. Add");
            Console.WriteLine("3. Edit");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Back");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            var vehicleRepo = new VehicleRepository();

            if (choice == "1")
            {
                var vehicles = vehicleRepo.GetAll();
                foreach (var v in vehicles)
                    Console.WriteLine($"[{v.Id}] {v.Name} ({v.Type}) | Rs.{v.PricePerDay}/day | {v.VehicleNumber} | {v.Status}");
            }
            else if (choice == "2")
            {
                var vehicle = new Vehicle();
                Console.Write("Name: "); vehicle.Name = Console.ReadLine();
                Console.Write("Type (Car/Bike/Scooter/SUV): "); vehicle.Type = Console.ReadLine();
                Console.Write("Price Per Day: "); vehicle.PricePerDay = decimal.Parse(Console.ReadLine());
                Console.Write("Vehicle Number: "); vehicle.VehicleNumber = Console.ReadLine();
                Console.Write("Status (Available/Lent/Disabled): "); vehicle.Status = Console.ReadLine();
                Console.Write("Description: "); vehicle.Description = Console.ReadLine();
                vehicleRepo.Add(vehicle);
                Console.WriteLine("Vehicle added.");
            }
            else if (choice == "3")
            {
                Console.Write("Enter Vehicle ID: ");
                int id = int.Parse(Console.ReadLine());
                var vehicle = vehicleRepo.GetById(id);
                if (vehicle == null) { Console.WriteLine("Not found."); continue; }
                
                Console.Write($"Name [{vehicle.Name}]: "); var name = Console.ReadLine();
                if (name != "") vehicle.Name = name;
                Console.Write($"Type [{vehicle.Type}]: "); var type = Console.ReadLine();
                if (type != "") vehicle.Type = type;
                Console.Write($"Price [{vehicle.PricePerDay}]: "); var price = Console.ReadLine();
                if (price != "") vehicle.PricePerDay = decimal.Parse(price);
                Console.Write($"Status [{vehicle.Status}]: "); var status = Console.ReadLine();
                if (status != "") vehicle.Status = status;
                vehicleRepo.Update(vehicle);
                Console.WriteLine("Vehicle updated.");
            }
            else if (choice == "4")
            {
                Console.Write("Enter Vehicle ID: ");
                int id = int.Parse(Console.ReadLine());
                vehicleRepo.Delete(id);
                Console.WriteLine("Vehicle deleted.");
            }
            else if (choice == "5") break;
        }
    }

    // ========== USERS ==========
    static void UsersMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- USERS ---");
            Console.WriteLine("1. List");
            Console.WriteLine("2. Add");
            Console.WriteLine("3. Edit");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Back");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            var userRepo = new UserRepository();

            if (choice == "1")
            {
                var users = userRepo.GetAll();
                foreach (var u in users)
                    Console.WriteLine($"[{u.Id}] {u.FirstName} {u.LastName} | {u.Email}");
            }
            else if (choice == "2")
            {
                var user = new User();
                Console.Write("First Name: "); user.FirstName = Console.ReadLine();
                Console.Write("Last Name: "); user.LastName = Console.ReadLine();
                Console.Write("Email: "); user.Email = Console.ReadLine();
                Console.Write("Password: "); user.Password = Console.ReadLine();
                userRepo.Add(user);
                Console.WriteLine("User added.");
            }
            else if (choice == "3")
            {
                Console.Write("Enter User ID: ");
                int id = int.Parse(Console.ReadLine());
                var user = userRepo.GetById(id);
                if (user == null) { Console.WriteLine("Not found."); continue; }
                
                Console.Write($"First Name [{user.FirstName}]: "); var fn = Console.ReadLine();
                if (fn != "") user.FirstName = fn;
                Console.Write($"Last Name [{user.LastName}]: "); var ln = Console.ReadLine();
                if (ln != "") user.LastName = ln;
                Console.Write($"Email [{user.Email}]: "); var email = Console.ReadLine();
                if (email != "") user.Email = email;
                Console.Write("Password: "); var pwd = Console.ReadLine();
                if (pwd != "") user.Password = pwd;
                userRepo.Update(user);
                Console.WriteLine("User updated.");
            }
            else if (choice == "4")
            {
                Console.Write("Enter User ID: ");
                int id = int.Parse(Console.ReadLine());
                userRepo.Delete(id);
                Console.WriteLine("User deleted.");
            }
            else if (choice == "5") break;
        }
    }

    // ========== ADMINS ==========
    static void AdminsMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- ADMINS ---");
            Console.WriteLine("1. List");
            Console.WriteLine("2. Add");
            Console.WriteLine("3. Edit");
            Console.WriteLine("4. Delete");
            Console.WriteLine("5. Back");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            var adminRepo = new AdminRepository();

            if (choice == "1")
            {
                var admins = adminRepo.GetAll();
                foreach (var a in admins)
                    Console.WriteLine($"[{a.Id}] {a.Username} | {a.Email}");
            }
            else if (choice == "2")
            {
                var admin = new Admin();
                Console.Write("Username: "); admin.Username = Console.ReadLine();
                Console.Write("Email: "); admin.Email = Console.ReadLine();
                Console.Write("Password: "); admin.Password = Console.ReadLine();
                adminRepo.Add(admin);
                Console.WriteLine("Admin added.");
            }
            else if (choice == "3")
            {
                Console.Write("Enter Admin ID: ");
                int id = int.Parse(Console.ReadLine());
                var admin = adminRepo.GetById(id);
                if (admin == null) { Console.WriteLine("Not found."); continue; }
                
                Console.Write($"Username [{admin.Username}]: "); var un = Console.ReadLine();
                if (un != "") admin.Username = un;
                Console.Write($"Email [{admin.Email}]: "); var email = Console.ReadLine();
                if (email != "") admin.Email = email;
                Console.Write("Password: "); var pwd = Console.ReadLine();
                if (pwd != "") admin.Password = pwd;
                adminRepo.Update(admin);
                Console.WriteLine("Admin updated.");
            }
            else if (choice == "4")
            {
                Console.Write("Enter Admin ID: ");
                int id = int.Parse(Console.ReadLine());
                adminRepo.Delete(id);
                Console.WriteLine("Admin deleted.");
            }
            else if (choice == "5") break;
        }
    }

    // ========== BOOKINGS ==========
    static void BookingsMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- BOOKINGS ---");
            Console.WriteLine("1. List");
            Console.WriteLine("2. Create");
            Console.WriteLine("3. Cancel");
            Console.WriteLine("4. Back");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                {
                    var bookingRepo = new BookingRepository();
                    var bookings = bookingRepo.GetAll();
                    foreach (var b in bookings)
                    {
                        int listDays = (b.EndDate - b.StartDate).Days + 1;
                        Console.WriteLine($"[{b.Id}] {b.VehicleName} | {b.UserName} | {b.StartDate:dd MMM} to {b.EndDate:dd MMM} ({listDays} days) | Rs.{b.TotalCost}");
                    }
                    break;
                }
                case "2":
                {
                    var vehicleRepo = new VehicleRepository();
                    var userRepo = new UserRepository();
                    var bookingRepo = new BookingRepository();
                    
                    Console.WriteLine("Available Vehicles:");
                    var vehicles = vehicleRepo.GetAvailable();
                    foreach (var v in vehicles)
                        Console.WriteLine($"[{v.Id}] {v.Name} ({v.Type}) | Rs.{v.PricePerDay}/day");

                    Console.Write("Vehicle ID: ");
                    int vehicleId = int.Parse(Console.ReadLine());
                    var vehicle = vehicleRepo.GetById(vehicleId);
                    
                    if (vehicle == null)
                    {
                        Console.WriteLine("Error: Vehicle not found!");
                        break;
                    }

                    Console.WriteLine("Users:");
                    var users = userRepo.GetAll();
                    foreach (var u in users)
                        Console.WriteLine($"[{u.Id}] {u.FirstName} {u.LastName}");

                    Console.Write("User ID: ");
                    int userId = int.Parse(Console.ReadLine());

                    Console.Write("Start Date (yyyy-mm-dd): ");
                    DateTime startDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("End Date (yyyy-mm-dd): ");
                    DateTime endDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Pickup Location: ");
                    string pickup = Console.ReadLine();

                    int bookingDays = (endDate - startDate).Days + 1;
                    decimal total = bookingDays * vehicle.PricePerDay + 500;

                    var booking = new Booking();
                    booking.UserId = userId;
                    booking.VehicleId = vehicleId;
                    booking.StartDate = startDate;
                    booking.EndDate = endDate;
                    booking.PickupLocation = pickup;
                    booking.TotalCost = total;

                    bookingRepo.Add(booking);
                    vehicleRepo.UpdateStatus(vehicleId, "Lent");
                    Console.WriteLine($"Booking created. Total: {total}");
                    break;
                }
                case "3":
                {
                    var bookingRepo = new BookingRepository();
                    var vehicleRepo = new VehicleRepository();
                    
                    Console.Write("Enter Booking ID: ");
                    int id = int.Parse(Console.ReadLine());
                    var booking = bookingRepo.GetById(id);
                    if (booking != null)
                    {
                        bookingRepo.Delete(id);
                        vehicleRepo.UpdateStatus(booking.VehicleId, "Available");
                        Console.WriteLine("Booking cancelled.");
                    }
                    break;
                }
                case "4":
                    return;
            }
        }
    }
}
