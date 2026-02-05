using System.Data.SQLite;

namespace GoWheelsConsole.Data;

public class SeedData
{
    public static void PopulateDummyData()
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();

        // Check and insert Admins
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Admins";
        var adminCount = Convert.ToInt32(cmd.ExecuteScalar());
        
        if (adminCount == 0)
        {
            cmd.CommandText = "INSERT INTO Admins (Username, Email, Password) VALUES ('admin', 'admin@gowheels.com', 'admin123')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Admins (Username, Email, Password) VALUES ('superadmin', 'superadmin@gowheels.com', 'super123')";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Admin dummy data inserted.");
        }
        else
        {
            Console.WriteLine("Admin data already exists.");
        }

        // Check and insert Users
        cmd.CommandText = "SELECT COUNT(*) FROM Users";
        var userCount = Convert.ToInt32(cmd.ExecuteScalar());
        
        if (userCount == 0)
        {
            cmd.CommandText = "INSERT INTO Users (FirstName, LastName, Email, Password) VALUES ('John', 'Doe', 'john@example.com', 'john123')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Users (FirstName, LastName, Email, Password) VALUES ('Jane', 'Smith', 'jane@example.com', 'jane123')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Users (FirstName, LastName, Email, Password) VALUES ('Mike', 'Johnson', 'mike@example.com', 'mike123')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Users (FirstName, LastName, Email, Password) VALUES ('Sarah', 'Williams', 'sarah@example.com', 'sarah123')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Users (FirstName, LastName, Email, Password) VALUES ('David', 'Brown', 'david@example.com', 'david123')";
            cmd.ExecuteNonQuery();
            Console.WriteLine("User dummy data inserted.");
        }
        else
        {
            Console.WriteLine("User data already exists.");
        }

        // Check and insert Vehicles
        cmd.CommandText = "SELECT COUNT(*) FROM Vehicles";
        var vehicleCount = Convert.ToInt32(cmd.ExecuteScalar());
        
        if (vehicleCount == 0)
        {
            cmd.CommandText = "INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description) VALUES ('Honda City', 'Car', 1500, 'MH01AB1234', 'Available', 'Comfortable sedan')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description) VALUES ('Maruti Swift', 'Car', 1200, 'MH02CD5678', 'Available', 'Compact hatchback')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description) VALUES ('Royal Enfield Classic', 'Bike', 800, 'MH03EF9012', 'Available', 'Classic motorcycle')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description) VALUES ('Honda Activa', 'Scooter', 400, 'MH04GH3456', 'Available', 'Popular scooter')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description) VALUES ('Mahindra Thar', 'SUV', 3000, 'MH05IJ7890', 'Available', 'Off-road SUV')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description) VALUES ('Toyota Innova', 'SUV', 2500, 'MH06KL1234', 'Lent', 'Family SUV')";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description) VALUES ('Yamaha FZ', 'Bike', 600, 'MH07MN5678', 'Available', 'Sports bike')";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Vehicle dummy data inserted.");
        }
        else
        {
            Console.WriteLine("Vehicle data already exists.");
        }

        // Check and insert Bookings
        cmd.CommandText = "SELECT COUNT(*) FROM Bookings";
        var bookingCount = Convert.ToInt32(cmd.ExecuteScalar());
        
        if (bookingCount == 0)
        {
            cmd.CommandText = "INSERT INTO Bookings (UserId, VehicleId, StartDate, EndDate, PickupLocation, TotalCost) VALUES (1, 6, '2026-02-01', '2026-02-05', 'Mumbai Airport', 13000)";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Bookings (UserId, VehicleId, StartDate, EndDate, PickupLocation, TotalCost) VALUES (2, 3, '2026-02-10', '2026-02-12', 'Pune Station', 2900)";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Booking dummy data inserted.");
        }
        else
        {
            Console.WriteLine("Booking data already exists.");
        }

        conn.Close();
    }
}
