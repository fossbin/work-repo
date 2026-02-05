using System.Data.SQLite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class UserRepository
{
    public List<User> GetAll()
    {
        var users = new List<User>();
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, FirstName, LastName, Email, Password FROM Users";
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var u = new User();
            u.Id = reader.GetInt32(0);
            u.FirstName = reader.GetString(1);
            u.LastName = reader.GetString(2);
            u.Email = reader.GetString(3);
            u.Password = reader.GetString(4);
            users.Add(u);
        }
        conn.Close();
        return users;
    }

    public User GetById(int id)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, FirstName, LastName, Email, Password FROM Users WHERE Id = " + id;
        var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var u = new User();
            u.Id = reader.GetInt32(0);
            u.FirstName = reader.GetString(1);
            u.LastName = reader.GetString(2);
            u.Email = reader.GetString(3);
            u.Password = reader.GetString(4);
            conn.Close();
            return u;
        }
        conn.Close();
        return null;
    }

    public void Add(User user)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"INSERT INTO Users (FirstName, LastName, Email, Password) VALUES ('{user.FirstName}', '{user.LastName}', '{user.Email}', '{user.Password}')";
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public void Update(User user)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"UPDATE Users SET FirstName='{user.FirstName}', LastName='{user.LastName}', Email='{user.Email}', Password='{user.Password}' WHERE Id={user.Id}";
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public void Delete(int id)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Users WHERE Id = " + id;
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public User GetByEmail(string email)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT Id, FirstName, LastName, Email, Password FROM Users WHERE Email = '{email}'";
        var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var u = new User();
            u.Id = reader.GetInt32(0);
            u.FirstName = reader.GetString(1);
            u.LastName = reader.GetString(2);
            u.Email = reader.GetString(3);
            u.Password = reader.GetString(4);
            conn.Close();
            return u;
        }
        conn.Close();
        return null;
    }
}
