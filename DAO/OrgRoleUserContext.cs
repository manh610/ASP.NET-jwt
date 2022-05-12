using MySql.Data.MySqlClient;

namespace WebApi.DAO;

public class OrgRoleUserContext
{
    public string ConnectionString { get; set; }

    public OrgRoleUserContext()
    {
        this.ConnectionString = "server=localhost; port=3306; database=ASP.NET; user=root; password=root";
    }

    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(ConnectionString);
    }

    public List<int> GetOrgRoleByUserId(int userId)
    {
        List<int> listId = new List<int>();
        using (MySqlConnection con = GetConnection())
        {
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select * from org_role_user where user_id = " + userId, con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int orgRoleId = Convert.ToInt32(reader["org_role_id"].ToString());
                    listId.Add(orgRoleId);
                }
                reader.Close();
            }
            con.Close();
        }
        return listId;
    }
}