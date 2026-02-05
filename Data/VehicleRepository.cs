using Microsoft.Data.Sqlite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class VehicleRepository
{
    public List<Vehicle> GetAll()
    {
        var vehicles = new List<Vehicle>();
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Type, PricePerDay, VehicleNumber, Status, Description FROM Vehicles";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            vehicles.Add(new Vehicle
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Type = reader.GetString(2),
                PricePerDay = reader.GetDecimal(3),
                VehicleNumber = reader.GetString(4),
                Status = reader.GetString(5),
                Description = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }
        return vehicles;
    }

    public List<Vehicle> GetAvailable()
    {
        var vehicles = new List<Vehicle>();
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Type, PricePerDay, VehicleNumber, Status, Description FROM Vehicles WHERE Status = 'Available'";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            vehicles.Add(new Vehicle
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Type = reader.GetString(2),
                PricePerDay = reader.GetDecimal(3),
                VehicleNumber = reader.GetString(4),
                Status = reader.GetString(5),
                Description = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }
        return vehicles;
    }

    public Vehicle? GetById(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Type, PricePerDay, VehicleNumber, Status, Description FROM Vehicles WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Vehicle
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Type = reader.GetString(2),
                PricePerDay = reader.GetDecimal(3),
                VehicleNumber = reader.GetString(4),
                Status = reader.GetString(5),
                Description = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }
        return null;
    }

    public void Add(Vehicle vehicle)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description)
            VALUES (@Name, @Type, @PricePerDay, @VehicleNumber, @Status, @Description)";
        command.Parameters.AddWithValue("@Name", vehicle.Name);
        command.Parameters.AddWithValue("@Type", vehicle.Type);
        command.Parameters.AddWithValue("@PricePerDay", vehicle.PricePerDay);
        command.Parameters.AddWithValue("@VehicleNumber", vehicle.VehicleNumber);
        command.Parameters.AddWithValue("@Status", vehicle.Status);
        command.Parameters.AddWithValue("@Description", vehicle.Description ?? (object)DBNull.Value);

        command.ExecuteNonQuery();
    }

    public void Update(Vehicle vehicle)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Vehicles SET 
                Name = @Name,
                Type = @Type,
                PricePerDay = @PricePerDay,
                VehicleNumber = @VehicleNumber,
                Status = @Status,
                Description = @Description
            WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", vehicle.Id);
        command.Parameters.AddWithValue("@Name", vehicle.Name);
        command.Parameters.AddWithValue("@Type", vehicle.Type);
        command.Parameters.AddWithValue("@PricePerDay", vehicle.PricePerDay);
        command.Parameters.AddWithValue("@VehicleNumber", vehicle.VehicleNumber);
        command.Parameters.AddWithValue("@Status", vehicle.Status);
        command.Parameters.AddWithValue("@Description", vehicle.Description ?? (object)DBNull.Value);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Vehicles WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }

    public void UpdateStatus(int id, string status)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Vehicles SET Status = @Status WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Status", status);

        command.ExecuteNonQuery();
    }
}
