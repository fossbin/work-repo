using Microsoft.Data.Sqlite;
using GoWheelsConsole.Models;

namespace GoWheelsConsole.Data;

public class AdminRepository
{
    public List<Admin> GetAll()
    {
        var admins = new List<Admin>();
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Username, Email, Password FROM Admins";
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var a = new Admin();
            a.Id = reader.GetInt32(0);
            a.Username = reader.GetString(1);
            a.Email = reader.GetString(2);
            a.Password = reader.GetString(3);
            admins.Add(a);
        }
        conn.Close();
        return admins;
    }

    public Admin GetById(int id)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Username, Email, Password FROM Admins WHERE Id = " + id;
        var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var a = new Admin();
            a.Id = reader.GetInt32(0);
            a.Username = reader.GetString(1);
            a.Email = reader.GetString(2);
            a.Password = reader.GetString(3);
            conn.Close();
            return a;
        }
        conn.Close();
        return null;
    }

    public void Add(Admin admin)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"INSERT INTO Admins (Username, Email, Password) VALUES ('{admin.Username}', '{admin.Email}', '{admin.Password}')";
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public void Update(Admin admin)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"UPDATE Admins SET Username='{admin.Username}', Email='{admin.Email}', Password='{admin.Password}' WHERE Id={admin.Id}";
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    public void Delete(int id)
    {
        var conn = DatabaseHelper.GetConnection();
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Admins WHERE Id = " + id;
        cmd.ExecuteNonQuery();
        conn.Close();
    }
}
