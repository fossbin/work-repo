using Microsoft.Data.Sqlite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class BookingRepository
{
    public List<Booking> GetAll()
    {
        var bookings = new List<Booking>();
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT b.Id, b.UserId, b.VehicleId, b.StartDate, b.EndDate, b.PickupLocation, b.TotalCost,
                   u.FirstName || ' ' || u.LastName as UserName, v.Name as VehicleName
            FROM Bookings b
            LEFT JOIN Users u ON b.UserId = u.Id
            LEFT JOIN Vehicles v ON b.VehicleId = v.Id";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            bookings.Add(new Booking
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                VehicleId = reader.GetInt32(2),
                StartDate = DateTime.Parse(reader.GetString(3)),
                EndDate = DateTime.Parse(reader.GetString(4)),
                PickupLocation = reader.GetString(5),
                TotalCost = reader.GetDecimal(6),
                UserName = reader.IsDBNull(7) ? null : reader.GetString(7),
                VehicleName = reader.IsDBNull(8) ? null : reader.GetString(8)
            });
        }
        return bookings;
    }

    public List<Booking> GetByUserId(int userId)
    {
        var bookings = new List<Booking>();
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT b.Id, b.UserId, b.VehicleId, b.StartDate, b.EndDate, b.PickupLocation, b.TotalCost,
                   u.FirstName || ' ' || u.LastName as UserName, v.Name as VehicleName
            FROM Bookings b
            LEFT JOIN Users u ON b.UserId = u.Id
            LEFT JOIN Vehicles v ON b.VehicleId = v.Id
            WHERE b.UserId = @UserId";
        command.Parameters.AddWithValue("@UserId", userId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            bookings.Add(new Booking
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                VehicleId = reader.GetInt32(2),
                StartDate = DateTime.Parse(reader.GetString(3)),
                EndDate = DateTime.Parse(reader.GetString(4)),
                PickupLocation = reader.GetString(5),
                TotalCost = reader.GetDecimal(6),
                UserName = reader.IsDBNull(7) ? null : reader.GetString(7),
                VehicleName = reader.IsDBNull(8) ? null : reader.GetString(8)
            });
        }
        return bookings;
    }

    public Booking? GetById(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT b.Id, b.UserId, b.VehicleId, b.StartDate, b.EndDate, b.PickupLocation, b.TotalCost,
                   u.FirstName || ' ' || u.LastName as UserName, v.Name as VehicleName
            FROM Bookings b
            LEFT JOIN Users u ON b.UserId = u.Id
            LEFT JOIN Vehicles v ON b.VehicleId = v.Id
            WHERE b.Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Booking
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                VehicleId = reader.GetInt32(2),
                StartDate = DateTime.Parse(reader.GetString(3)),
                EndDate = DateTime.Parse(reader.GetString(4)),
                PickupLocation = reader.GetString(5),
                TotalCost = reader.GetDecimal(6),
                UserName = reader.IsDBNull(7) ? null : reader.GetString(7),
                VehicleName = reader.IsDBNull(8) ? null : reader.GetString(8)
            };
        }
        return null;
    }

    public void Add(Booking booking)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Bookings (UserId, VehicleId, StartDate, EndDate, PickupLocation, TotalCost)
            VALUES (@UserId, @VehicleId, @StartDate, @EndDate, @PickupLocation, @TotalCost)";
        command.Parameters.AddWithValue("@UserId", booking.UserId);
        command.Parameters.AddWithValue("@VehicleId", booking.VehicleId);
        command.Parameters.AddWithValue("@StartDate", booking.StartDate.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@EndDate", booking.EndDate.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@PickupLocation", booking.PickupLocation);
        command.Parameters.AddWithValue("@TotalCost", booking.TotalCost);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Bookings WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }
}
