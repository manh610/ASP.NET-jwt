using MySql.Data.MySqlClient;
using WebApi.Entities;

namespace WebApi.DAO;

public class SystemRoleUserContext
{
    public string ConnectionString { get; set; }

    public SystemRoleUserContext()
    {
        this.ConnectionString = "server=localhost; port=3306; database=ASP.NET; user=root; password=root";
    }

    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(ConnectionString);
    }

    public List<int> GetSystemRoleByUserId(int userId)
    {
        List<int> listId = new List<int>();
        using (MySqlConnection con = GetConnection())
        {
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select * from system_role_user where user_id = " + userId, con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int SystemRoleId = Convert.ToInt32(reader["system_role_id"].ToString());
                    listId.Add(SystemRoleId);
                }
                reader.Close();
            }
            con.Close();
        }
        return listId;
    }
}