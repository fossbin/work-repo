using Microsoft.Data.Sqlite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class UserRepository
{
    public List<User> GetAll()
    {
        var users = new List<User>();
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, FirstName, LastName, Email, Password FROM Users";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Password = reader.GetString(4)
            });
        }
        return users;
    }

    public User? GetById(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, FirstName, LastName, Email, Password FROM Users WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Password = reader.GetString(4)
            };
        }
        return null;
    }

    public void Add(User user)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users (FirstName, LastName, Email, Password)
            VALUES (@FirstName, @LastName, @Email, @Password)";
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Password", user.Password);

        command.ExecuteNonQuery();
    }

    public void Update(User user)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Users SET 
                FirstName = @FirstName,
                LastName = @LastName,
                Email = @Email,
                Password = @Password
            WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@FirstName", user.FirstName);
        command.Parameters.AddWithValue("@LastName", user.LastName);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Password", user.Password);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = DatabaseHelper.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Users WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }
}
