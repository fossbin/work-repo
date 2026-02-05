using GoWheelsConsole.Data;
using GoWheelsConsole.Models;

namespace GoWheelsConsole;

class Program
{
    static UserRepository userRepo = new UserRepository();
    static VehicleRepository vehicleRepo = new VehicleRepository();
    static BookingRepository bookingRepo = new BookingRepository();
    static AdminRepository adminRepo = new AdminRepository();
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

            switch (choice)
            {
                case "1":
                    UserLogin();
                    break;
                case "2":
                    UserRegistration();
                    break;
                case "3":
                    AdminLogin();
                    break;
                case "4":
                    return;
            }
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

            switch (choice)
            {
                case "1":
                    BrowseVehicles();
                    break;
                case "2":
                    MyBookings();
                    break;
                case "3":
                    CreateBooking();
                    break;
                case "4":
                    CancelBooking();
                    break;
                case "5":
                    currentUser = null;
                    Console.WriteLine("Logged out successfully!");
                    break;
            }
        }
    }

    static void BrowseVehicles()
    {
        Console.WriteLine("\n--- AVAILABLE VEHICLES ---");
        var vehicles = vehicleRepo.GetAvailable();
        foreach (var v in vehicles)
            Console.WriteLine($"[{v.Id}] {v.Name} ({v.Type}) | Rs.{v.PricePerDay}/day | {v.VehicleNumber}");
    }

    static void MyBookings()
    {
        Console.WriteLine("\n--- MY BOOKINGS ---");
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
        var vehicles = vehicleRepo.GetAvailable();
        foreach (var v in vehicles)
            Console.WriteLine($"[{v.Id}] {v.Name} ({v.Type}) | Rs.{v.PricePerDay}/day");

        Console.Write("Vehicle ID: ");
        int vehicleId = int.Parse(Console.ReadLine());
        var vehicle = vehicleRepo.GetById(vehicleId);

        if (vehicle == null || vehicle.Status != "Available")
        {
            Console.WriteLine("Vehicle not available!");
            return;
        }

        Console.Write("Start Date (yyyy-mm-dd): ");
        DateTime startDate = DateTime.Parse(Console.ReadLine());
        Console.Write("End Date (yyyy-mm-dd): ");
        DateTime endDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Pickup Location: ");
        string pickup = Console.ReadLine();

        int days = (endDate - startDate).Days + 1;
        decimal total = days * vehicle.PricePerDay + 500;

        var booking = new Booking();
        booking.UserId = currentUser.Id;
        booking.VehicleId = vehicleId;
        booking.StartDate = startDate;
        booking.EndDate = endDate;
        booking.PickupLocation = pickup;
        booking.TotalCost = total;

        bookingRepo.Add(booking);
        vehicleRepo.UpdateStatus(vehicleId, "Lent");
        Console.WriteLine($"Booking created. Total: Rs.{total}");
    }

    static void CancelBooking()
    {
        Console.WriteLine("\n--- CANCEL BOOKING ---");
        Console.Write("Enter Booking ID: ");
        int id = int.Parse(Console.ReadLine());
        var booking = bookingRepo.GetById(id);
        
        if (booking == null)
        {
            Console.WriteLine("Booking not found!");
            return;
        }

        if (booking.UserId != currentUser.Id)
        {
            Console.WriteLine("You can only cancel your own bookings!");
            return;
        }

        bookingRepo.Delete(id);
        vehicleRepo.UpdateStatus(booking.VehicleId, "Available");
        Console.WriteLine("Booking cancelled.");
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
                    currentAdmin = null;
                    Console.WriteLine("Logged out successfully!");
                    break;
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

            switch (choice)
            {
                case "1":
                    var vehicles = vehicleRepo.GetAll();
                    foreach (var v in vehicles)
                        Console.WriteLine($"[{v.Id}] {v.Name} ({v.Type}) | Rs.{v.PricePerDay}/day | {v.VehicleNumber} | {v.Status}");
                    break;
                case "2":
                    var vehicle = new Vehicle();
                    Console.Write("Name: "); vehicle.Name = Console.ReadLine();
                    Console.Write("Type (Car/Bike/Scooter/SUV): "); vehicle.Type = Console.ReadLine();
                    Console.Write("Price Per Day: "); vehicle.PricePerDay = decimal.Parse(Console.ReadLine());
                    Console.Write("Vehicle Number: "); vehicle.VehicleNumber = Console.ReadLine();
                    Console.Write("Status (Available/Lent/Disabled): "); vehicle.Status = Console.ReadLine();
                    Console.Write("Description: "); vehicle.Description = Console.ReadLine();
                    vehicleRepo.Add(vehicle);
                    Console.WriteLine("Vehicle added.");
                    break;
                case "3":
                    Console.Write("Enter Vehicle ID: ");
                    int id = int.Parse(Console.ReadLine());
                    var vehicleToEdit = vehicleRepo.GetById(id);
                    if (vehicleToEdit == null) { Console.WriteLine("Not found."); break; }
                    
                    Console.Write($"Name [{vehicleToEdit.Name}]: "); var name = Console.ReadLine();
                    if (name != "") vehicleToEdit.Name = name;
                    Console.Write($"Type [{vehicleToEdit.Type}]: "); var type = Console.ReadLine();
                    if (type != "") vehicleToEdit.Type = type;
                    Console.Write($"Price [{vehicleToEdit.PricePerDay}]: "); var price = Console.ReadLine();
                    if (price != "") vehicleToEdit.PricePerDay = decimal.Parse(price);
                    Console.Write($"Status [{vehicleToEdit.Status}]: "); var status = Console.ReadLine();
                    if (status != "") vehicleToEdit.Status = status;
                    vehicleRepo.Update(vehicleToEdit);
                    Console.WriteLine("Vehicle updated.");
                    break;
                case "4":
                    Console.Write("Enter Vehicle ID: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    vehicleRepo.Delete(deleteId);
                    Console.WriteLine("Vehicle deleted.");
                    break;
                case "5":
                    return;
            }
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

            switch (choice)
            {
                case "1":
                    var users = userRepo.GetAll();
                    foreach (var u in users)
                        Console.WriteLine($"[{u.Id}] {u.FirstName} {u.LastName} | {u.Email}");
                    break;
                case "2":
                    var user = new User();
                    Console.Write("First Name: "); user.FirstName = Console.ReadLine();
                    Console.Write("Last Name: "); user.LastName = Console.ReadLine();
                    Console.Write("Email: "); user.Email = Console.ReadLine();
                    Console.Write("Password: "); user.Password = Console.ReadLine();
                    userRepo.Add(user);
                    Console.WriteLine("User added.");
                    break;
                case "3":
                    Console.Write("Enter User ID: ");
                    int id = int.Parse(Console.ReadLine());
                    var userToEdit = userRepo.GetById(id);
                    if (userToEdit == null) { Console.WriteLine("Not found."); break; }
                    
                    Console.Write($"First Name [{userToEdit.FirstName}]: "); var fn = Console.ReadLine();
                    if (fn != "") userToEdit.FirstName = fn;
                    Console.Write($"Last Name [{userToEdit.LastName}]: "); var ln = Console.ReadLine();
                    if (ln != "") userToEdit.LastName = ln;
                    Console.Write($"Email [{userToEdit.Email}]: "); var email = Console.ReadLine();
                    if (email != "") userToEdit.Email = email;
                    Console.Write("Password: "); var pwd = Console.ReadLine();
                    if (pwd != "") userToEdit.Password = pwd;
                    userRepo.Update(userToEdit);
                    Console.WriteLine("User updated.");
                    break;
                case "4":
                    Console.Write("Enter User ID: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    userRepo.Delete(deleteId);
                    Console.WriteLine("User deleted.");
                    break;
                case "5":
                    return;
            }
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

            switch (choice)
            {
                case "1":
                    var admins = adminRepo.GetAll();
                    foreach (var a in admins)
                        Console.WriteLine($"[{a.Id}] {a.Username} | {a.Email}");
                    break;
                case "2":
                    var admin = new Admin();
                    Console.Write("Username: "); admin.Username = Console.ReadLine();
                    Console.Write("Email: "); admin.Email = Console.ReadLine();
                    Console.Write("Password: "); admin.Password = Console.ReadLine();
                    adminRepo.Add(admin);
                    Console.WriteLine("Admin added.");
                    break;
                case "3":
                    Console.Write("Enter Admin ID: ");
                    int id = int.Parse(Console.ReadLine());
                    var adminToEdit = adminRepo.GetById(id);
                    if (adminToEdit == null) { Console.WriteLine("Not found."); break; }
                    
                    Console.Write($"Username [{adminToEdit.Username}]: "); var un = Console.ReadLine();
                    if (un != "") adminToEdit.Username = un;
                    Console.Write($"Email [{adminToEdit.Email}]: "); var email = Console.ReadLine();
                    if (email != "") adminToEdit.Email = email;
                    Console.Write("Password: "); var pwd = Console.ReadLine();
                    if (pwd != "") adminToEdit.Password = pwd;
                    adminRepo.Update(adminToEdit);
                    Console.WriteLine("Admin updated.");
                    break;
                case "4":
                    Console.Write("Enter Admin ID: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    adminRepo.Delete(deleteId);
                    Console.WriteLine("Admin deleted.");
                    break;
                case "5":
                    return;
            }
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
                    var bookings = bookingRepo.GetAll();
                    foreach (var b in bookings)
                    {
                        int days = (b.EndDate - b.StartDate).Days + 1;
                        Console.WriteLine($"[{b.Id}] {b.VehicleName} | {b.UserName} | {b.StartDate:dd MMM} to {b.EndDate:dd MMM} ({days} days) | Rs.{b.TotalCost}");
                    }
                    break;
                }
                case "2":
                {
                    Console.WriteLine("Available Vehicles:");
                    var vehicles = vehicleRepo.GetAvailable();
                    foreach (var v in vehicles)
                        Console.WriteLine($"[{v.Id}] {v.Name} ({v.Type}) | Rs.{v.PricePerDay}/day");

                    Console.Write("Vehicle ID: ");
                    int vehicleId = int.Parse(Console.ReadLine());
                    var vehicle = vehicleRepo.GetById(vehicleId);

                    if (vehicle == null)
                    {
                        Console.WriteLine("Vehicle not found!");
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

                    int days = (endDate - startDate).Days + 1;
                    decimal total = days * vehicle.PricePerDay + 500;

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
                    Console.Write("Enter Booking ID: ");
                    int id = int.Parse(Console.ReadLine());
                    var bookingToCancel = bookingRepo.GetById(id);
                    if (bookingToCancel != null)
                    {
                        bookingRepo.Delete(id);
                        vehicleRepo.UpdateStatus(bookingToCancel.VehicleId, "Available");
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
