using Microsoft.Data.Sqlite;

namespace GoWheelsConsole.Data;

public class DatabaseHelper
{
    static string connectionString = "Data Source=gowheels.db";

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(connectionString);
    }

    public static void InitializeDatabase()
    {
        var connection = GetConnection();
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Admins (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT,
                Email TEXT,
                Password TEXT
            );
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT,
                LastName TEXT,
                Email TEXT,
                Password TEXT
            );
            CREATE TABLE IF NOT EXISTS Vehicles (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT,
                Type TEXT,
                PricePerDay REAL,
                VehicleNumber TEXT,
                Status TEXT,
                Description TEXT
            );
            CREATE TABLE IF NOT EXISTS Bookings (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER,
                VehicleId INTEGER,
                StartDate TEXT,
                EndDate TEXT,
                PickupLocation TEXT,
                TotalCost REAL
            );";
        cmd.ExecuteNonQuery();
        connection.Close();
        Console.WriteLine("Database ready.");
    }
}
