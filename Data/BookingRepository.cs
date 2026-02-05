using System.Data.SQLite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class BookingRepository
{
    public List<Booking> GetAll()
    {
        var bookings = new List<Booking>();
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT b.Id, b.UserId, b.VehicleId, b.StartDate, b.EndDate, b.PickupLocation, b.TotalCost, 
            u.FirstName || ' ' || u.LastName, v.Name 
            FROM Bookings b 
            LEFT JOIN Users u ON b.UserId = u.Id 
            LEFT JOIN Vehicles v ON b.VehicleId = v.Id";
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var b = new Booking();
            b.Id = reader.GetInt32(0);
            b.UserId = reader.GetInt32(1);
            b.VehicleId = reader.GetInt32(2);
            b.StartDate = DateTime.Parse(reader.GetString(3));
            b.EndDate = DateTime.Parse(reader.GetString(4));
            b.PickupLocation = reader.GetString(5);
            b.TotalCost = reader.GetDecimal(6);
            b.UserName = reader.IsDBNull(7) ? "" : reader.GetString(7);
            b.VehicleName = reader.IsDBNull(8) ? "" : reader.GetString(8);
            bookings.Add(b);
        }
        conn.Close();
        return bookings;
    }

    public Booking GetById(int id)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, UserId, VehicleId, StartDate, EndDate, PickupLocation, TotalCost FROM Bookings WHERE Id = " + id;
        var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var b = new Booking();
            b.Id = reader.GetInt32(0);
            b.UserId = reader.GetInt32(1);
            b.VehicleId = reader.GetInt32(2);
            b.StartDate = DateTime.Parse(reader.GetString(3));
            b.EndDate = DateTime.Parse(reader.GetString(4));
            b.PickupLocation = reader.GetString(5);
            b.TotalCost = reader.GetDecimal(6);
            conn.Close();
            return b;
        }
        conn.Close();
        return null;
    }

    public void Add(Booking b)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"INSERT INTO Bookings (UserId, VehicleId, StartDate, EndDate, PickupLocation, TotalCost) VALUES ({b.UserId}, {b.VehicleId}, '{b.StartDate:yyyy-MM-dd}', '{b.EndDate:yyyy-MM-dd}', '{b.PickupLocation}', {b.TotalCost})";
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public void Delete(int id)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Bookings WHERE Id = " + id;
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public List<Booking> GetByUserId(int userId)
    {
        var bookings = new List<Booking>();
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $@"SELECT b.Id, b.UserId, b.VehicleId, b.StartDate, b.EndDate, b.PickupLocation, b.TotalCost, 
            u.FirstName || ' ' || u.LastName, v.Name 
            FROM Bookings b 
            LEFT JOIN Users u ON b.UserId = u.Id 
            LEFT JOIN Vehicles v ON b.VehicleId = v.Id
            WHERE b.UserId = {userId}";
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var b = new Booking();
            b.Id = reader.GetInt32(0);
            b.UserId = reader.GetInt32(1);
            b.VehicleId = reader.GetInt32(2);
            b.StartDate = DateTime.Parse(reader.GetString(3));
            b.EndDate = DateTime.Parse(reader.GetString(4));
            b.PickupLocation = reader.GetString(5);
            b.TotalCost = reader.GetDecimal(6);
            b.UserName = reader.IsDBNull(7) ? "" : reader.GetString(7);
            b.VehicleName = reader.IsDBNull(8) ? "" : reader.GetString(8);
            bookings.Add(b);
        }
        conn.Close();
        return bookings;
    }
}
