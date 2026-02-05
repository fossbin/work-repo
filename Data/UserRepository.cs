using System.Data.SQLite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class UserRepository
{
    public List<User> GetAll()
    {
        var users = new List<User>();
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, FirstName, LastName, Email, Password FROM Users";
                using (var reader = cmd.ExecuteReader())
                {
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
                }
            }
        }
        return users;
    }

    public User GetById(int id)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, FirstName, LastName, Email, Password FROM Users WHERE Id = " + id;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var u = new User();
                        u.Id = reader.GetInt32(0);
                        u.FirstName = reader.GetString(1);
                        u.LastName = reader.GetString(2);
                        u.Email = reader.GetString(3);
                        u.Password = reader.GetString(4);
                        return u;
                    }
                }
            }
        }
        return null;
    }

    public void Add(User user)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO Users (FirstName, LastName, Email, Password) VALUES ('{user.FirstName}', '{user.LastName}', '{user.Email}', '{user.Password}')";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Update(User user)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"UPDATE Users SET FirstName='{user.FirstName}', LastName='{user.LastName}', Email='{user.Email}', Password='{user.Password}' WHERE Id={user.Id}";
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
                cmd.CommandText = "DELETE FROM Users WHERE Id = " + id;
                cmd.ExecuteNonQuery();
            }
        }
    }

    public User GetByEmail(string email)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT Id, FirstName, LastName, Email, Password FROM Users WHERE Email = '{email}'";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var u = new User();
                        u.Id = reader.GetInt32(0);
                        u.FirstName = reader.GetString(1);
                        u.LastName = reader.GetString(2);
                        u.Email = reader.GetString(3);
                        u.Password = reader.GetString(4);
                        return u;
                    }
                }
            }
        }
        return null;
    }
}
