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

        // Create Admins table
        var createAdminsTable = connection.CreateCommand();
        createAdminsTable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Admins (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Email TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL
            )";
        createAdminsTable.ExecuteNonQuery();

        // Create Users table
        var createUsersTable = connection.CreateCommand();
        createUsersTable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL
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

        Console.WriteLine("Database initialized.");
    }
}
