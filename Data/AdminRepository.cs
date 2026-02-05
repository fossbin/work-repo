using System.Data.SQLite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class AdminRepository
{
    public List<Admin> GetAll()
    {
        var admins = new List<Admin>();
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Username, Email, Password FROM Admins";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var a = new Admin();
                        a.Id = reader.GetInt32(0);
                        a.Username = reader.GetString(1);
                        a.Email = reader.GetString(2);
                        a.Password = reader.GetString(3);
                        admins.Add(a);
                    }
                }
            }
        }
        return admins;
    }

    public Admin GetById(int id)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Username, Email, Password FROM Admins WHERE Id = " + id;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var a = new Admin();
                        a.Id = reader.GetInt32(0);
                        a.Username = reader.GetString(1);
                        a.Email = reader.GetString(2);
                        a.Password = reader.GetString(3);
                        return a;
                    }
                }
            }
        }
        return null;
    }

    public void Add(Admin admin)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO Admins (Username, Email, Password) VALUES ('{admin.Username}', '{admin.Email}', '{admin.Password}')";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Update(Admin admin)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"UPDATE Admins SET Username='{admin.Username}', Email='{admin.Email}', Password='{admin.Password}' WHERE Id={admin.Id}";
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
                cmd.CommandText = "DELETE FROM Admins WHERE Id = " + id;
                cmd.ExecuteNonQuery();
            }
        }
    }

    public Admin GetByUsername(string username)
    {
        using (var conn = DatabaseHelper.GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"SELECT Id, Username, Email, Password FROM Admins WHERE Username = '{username}'";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var a = new Admin();
                        a.Id = reader.GetInt32(0);
                        a.Username = reader.GetString(1);
                        a.Email = reader.GetString(2);
                        a.Password = reader.GetString(3);
                        return a;
                    }
                }
            }
        }
        return null;
    }
}
