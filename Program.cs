using GoWheelsConsole.Data;
using GoWheelsConsole.Models;

namespace GoWheelsConsole;

class Program
{
    static UserRepository userRepo = new UserRepository();
    static VehicleRepository vehicleRepo = new VehicleRepository();
    static BookingRepository bookingRepo = new BookingRepository();
    static AdminRepository adminRepo = new AdminRepository();

    static void Main(string[] args)
    {
        DatabaseHelper.InitializeDatabase();

        while (true)
        {
            Console.WriteLine("\n=== GOWHEELS MENU ===");
            Console.WriteLine("1. Vehicles");
            Console.WriteLine("2. Users");
            Console.WriteLine("3. Admins");
            Console.WriteLine("4. Bookings");
            Console.WriteLine("5. Exit");
            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            if (choice == "1") VehiclesMenu();
            else if (choice == "2") UsersMenu();
            else if (choice == "3") AdminsMenu();
            else if (choice == "4") BookingsMenu();
            else if (choice == "5") break;
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

            if (choice == "1")
            {
                var bookings = bookingRepo.GetAll();
                foreach (var b in bookings)
                {
                    int days = (b.EndDate - b.StartDate).Days + 1;
                    Console.WriteLine($"[{b.Id}] {b.VehicleName} | {b.UserName} | {b.StartDate:dd MMM} to {b.EndDate:dd MMM} ({days} days) | Rs.{b.TotalCost}");
                }
            }
            else if (choice == "2")
            {
                Console.WriteLine("Available Vehicles:");
                var vehicles = vehicleRepo.GetAvailable();
                foreach (var v in vehicles)
                    Console.WriteLine($"[{v.Id}] {v.Name} ({v.Type}) | Rs.{v.PricePerDay}/day");

                Console.Write("Vehicle ID: ");
                int vehicleId = int.Parse(Console.ReadLine());
                var vehicle = vehicleRepo.GetById(vehicleId);

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
            }
            else if (choice == "3")
            {
                Console.Write("Enter Booking ID: ");
                int id = int.Parse(Console.ReadLine());
                var booking = bookingRepo.GetById(id);
                if (booking != null)
                {
                    bookingRepo.Delete(id);
                    vehicleRepo.UpdateStatus(booking.VehicleId, "Available");
                    Console.WriteLine("Booking cancelled.");
                }
            }
            else if (choice == "4") break;
        }
    }
}
