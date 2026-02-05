using Microsoft.Data.Sqlite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class AdminRepository
{
    public List<Admin> GetAll()
    {
        var admins = new List<Admin>();
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Username, Email, Password FROM Admins";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            admins.Add(new Admin
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3)
            });
        }
        return admins;
    }

    public Admin? GetById(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Username, Email, Password FROM Admins WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Admin
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3)
            };
        }
        return null;
    }

    public Admin? GetByUsername(string username)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Username, Email, Password FROM Admins WHERE Username = @Username";
        command.Parameters.AddWithValue("@Username", username);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Admin
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3)
            };
        }
        return null;
    }

    public void Add(Admin admin)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Admins (Username, Email, Password)
            VALUES (@Username, @Email, @Password)";
        command.Parameters.AddWithValue("@Username", admin.Username);
        command.Parameters.AddWithValue("@Email", admin.Email);
        command.Parameters.AddWithValue("@Password", admin.Password);

        command.ExecuteNonQuery();
    }

    public void Update(Admin admin)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Admins SET 
                Username = @Username,
                Email = @Email,
                Password = @Password
            WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", admin.Id);
        command.Parameters.AddWithValue("@Username", admin.Username);
        command.Parameters.AddWithValue("@Email", admin.Email);
        command.Parameters.AddWithValue("@Password", admin.Password);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Admins WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }
}
