using MySql.Data.MySqlClient;
using WebApi.Entities;

namespace WebApi.DAO;

public class OrgRoleContext
{
    public string ConnectionString { get; set; }

    public OrgRoleContext()
    {
        this.ConnectionString = "server=localhost; port=3306; database=ASP.NET; user=root; password=root";
    }

    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(ConnectionString);
    }

    public OrgRole GetOrgRoleById(int id)
    {
        OrgRole orgRole = new OrgRole();

        using (MySqlConnection con = GetConnection())
        {
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select * from org_role where id = " + id, con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string role = reader["role"].ToString();
                    orgRole.Id = id;
                    orgRole.Role = role;
                }
                reader.Close();
            }
            con.Close();
        }
        return orgRole;
    }
}