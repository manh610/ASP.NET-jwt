using MySql.Data.MySqlClient;
using WebApi.Entities;

namespace WebApi.DAO;


public class SystemRoleContext
{
    public string ConnectionString { get; set; }

    public SystemRoleContext()
    {
        this.ConnectionString = "server=localhost; port=3306; database=ASP.NET; user=root; password=root";
    }

    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(ConnectionString);
    }

    public SystemRole GetSystemRoleById(int id)
    {
        SystemRole systemRole = new SystemRole();
        using (MySqlConnection con = GetConnection())
        {
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select * from system_role where id = " + id, con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string role = reader["role"].ToString();
                    systemRole.Id = id;
                    systemRole.Role = role;
                }
                reader.Close();
            }
            con.Close();
        }
        return systemRole;
    }
}