using Microsoft.Data.Sqlite;

namespace GoWheelsConsole.Data;

public static class DatabaseHelper
{
    private static readonly string ConnectionString = "Data Source=gowheels.db";

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(ConnectionString);
    }

    public static void InitializeDatabase()
    {
        using var connection = GetConnection();
        connection.Open();

        // Create Users table
        var createUsersTable = connection.CreateCommand();
        createUsersTable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                Role TEXT NOT NULL DEFAULT 'user'
            )";
        createUsersTable.ExecuteNonQuery();

        // Create Vehicles table
        var createVehiclesTable = connection.CreateCommand();
        createVehiclesTable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Vehicles (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Type TEXT NOT NULL,
                PricePerDay REAL NOT NULL,
                VehicleNumber TEXT NOT NULL UNIQUE,
                Status TEXT NOT NULL DEFAULT 'Available',
                Description TEXT
            )";
        createVehiclesTable.ExecuteNonQuery();

        // Create Bookings table
        var createBookingsTable = connection.CreateCommand();
        createBookingsTable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Bookings (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                VehicleId INTEGER NOT NULL,
                StartDate TEXT NOT NULL,
                EndDate TEXT NOT NULL,
                PickupLocation TEXT NOT NULL,
                TotalCost REAL NOT NULL,
                FOREIGN KEY (UserId) REFERENCES Users(Id),
                FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id)
            )";
        createBookingsTable.ExecuteNonQuery();

        // Seed sample data if tables are empty
        SeedData(connection);
    }

    private static void SeedData(SqliteConnection connection)
    {
        // Check if data already exists
        var checkUsers = connection.CreateCommand();
        checkUsers.CommandText = "SELECT COUNT(*) FROM Users";
        var userCount = Convert.ToInt64(checkUsers.ExecuteScalar());

        if (userCount == 0)
        {
            // Seed Users
            var insertUsers = connection.CreateCommand();
            insertUsers.CommandText = @"
                INSERT INTO Users (FirstName, LastName, Email, Password, Role) VALUES
                ('Admin', 'User', 'admin@gowheels.com', 'Admin@123', 'admin'),
                ('Steve', 'Fernandez', 'steve@gowheels.com', 'Steve@123', 'user'),
                ('John', 'Doe', 'john@example.com', 'John@123', 'user')";
            insertUsers.ExecuteNonQuery();

            // Seed Vehicles
            var insertVehicles = connection.CreateCommand();
            insertVehicles.CommandText = @"
                INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description) VALUES
                ('Honda City', 'Car', 2500, 'KA01AB1234', 'Available', 'Premium sedan with great mileage'),
                ('Royal Enfield Classic', 'Bike', 800, 'KA01CD5678', 'Available', 'Classic cruiser bike'),
                ('Hyundai Creta', 'SUV', 3000, 'KA01EF9012', 'Available', 'Compact SUV with premium features'),
                ('TVS Apache RTR', 'Bike', 700, 'KA01GH3456', 'Disabled', 'Sports bike'),
                ('Honda Activa', 'Scooter', 500, 'KA01IJ7890', 'Available', 'Reliable scooter for city commute'),
                ('Maruti Swift', 'Car', 1800, 'KA01KL2345', 'Available', 'Compact hatchback')";
            insertVehicles.ExecuteNonQuery();

            Console.WriteLine("Database initialized with sample data.");
        }
    }
}
