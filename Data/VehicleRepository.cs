using System.Data.SQLite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class VehicleRepository
{
    public List<Vehicle> GetAll()
    {
        var vehicles = new List<Vehicle>();
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Name, Type, PricePerDay, VehicleNumber, Status, Description FROM Vehicles";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var v = new Vehicle();
                        v.Id = reader.GetInt32(0);
                        v.Name = reader.GetString(1);
                        v.Type = reader.GetString(2);
                        v.PricePerDay = reader.GetDecimal(3);
                        v.VehicleNumber = reader.GetString(4);
                        v.Status = reader.GetString(5);
                        v.Description = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        vehicles.Add(v);
                    }
                }
            }
        }
        return vehicles;
    }

    public List<Vehicle> GetAvailable()
    {
        var vehicles = new List<Vehicle>();
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Name, Type, PricePerDay, VehicleNumber, Status, Description FROM Vehicles WHERE Status = 'Available'";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var v = new Vehicle();
                        v.Id = reader.GetInt32(0);
                        v.Name = reader.GetString(1);
                        v.Type = reader.GetString(2);
                        v.PricePerDay = reader.GetDecimal(3);
                        v.VehicleNumber = reader.GetString(4);
                        v.Status = reader.GetString(5);
                        v.Description = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        vehicles.Add(v);
                    }
                }
            }
        }
        return vehicles;
    }

    public Vehicle GetById(int id)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Name, Type, PricePerDay, VehicleNumber, Status, Description FROM Vehicles WHERE Id = " + id;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var v = new Vehicle();
                        v.Id = reader.GetInt32(0);
                        v.Name = reader.GetString(1);
                        v.Type = reader.GetString(2);
                        v.PricePerDay = reader.GetDecimal(3);
                        v.VehicleNumber = reader.GetString(4);
                        v.Status = reader.GetString(5);
                        v.Description = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        return v;
                    }
                }
            }
        }
        return null;
    }

    public void Add(Vehicle v)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO Vehicles (Name, Type, PricePerDay, VehicleNumber, Status, Description) VALUES ('{v.Name}', '{v.Type}', {v.PricePerDay}, '{v.VehicleNumber}', '{v.Status}', '{v.Description}')";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Update(Vehicle v)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"UPDATE Vehicles SET Name='{v.Name}', Type='{v.Type}', PricePerDay={v.PricePerDay}, VehicleNumber='{v.VehicleNumber}', Status='{v.Status}', Description='{v.Description}' WHERE Id={v.Id}";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Delete(int id)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Vehicles WHERE Id = " + id;
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void UpdateStatus(int id, string status)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"UPDATE Vehicles SET Status='{status}' WHERE Id={id}";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
