using MySql.Data.MySqlClient;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.DAO;
public class UserContext
{
    public string ConnectionString { get; set; }


    public UserContext()
    {
        this.ConnectionString = "server=localhost; port=3306; database=ASP.NET; user=root; password=root";
    }

    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(ConnectionString);
    }

    public List<User> GetAllUser()
    {
        List<User> list = new List<User>();
        using (MySqlConnection con = GetConnection())
        {
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select * from user", con);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"].ToString());
                    string username = reader["username"].ToString();
                    string password = reader["password"].ToString();
                    string FirstName = reader["firstname"].ToString();
                    string LastName = reader["lastname"].ToString();
                    User user = new User(id, FirstName, LastName, username, password);
                    list.Add(user);
                }
                reader.Close();
            }
            con.Close();
        }
        return list;
    }


    public User GetUserById(int id)
    {
        User user = new User();
        try
        {
            using (MySqlConnection con = GetConnection())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user where id = " + id, con);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string username = reader["username"].ToString();
                            string password = reader["password"].ToString();
                            string FirstName = reader["firstname"].ToString();
                            string LastName = reader["lastname"].ToString();
                            user = new User(id, FirstName, LastName, username, password);
                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        con.Close();
                        return null;
                    }
                }
                con.Close();
            }
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return null;
        }

    }

    public User GetUserByUsername(string username)
    {
        User user = new User();
        try
        {
            using (MySqlConnection con = GetConnection())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user where username = " + "\"" + username + "\"", con);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"].ToString());
                            string password = reader["password"].ToString();
                            string FirstName = reader["firstname"].ToString();
                            string LastName = reader["lastname"].ToString();
                            user = new User(id, FirstName, LastName, username, password);
                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        con.Close();
                        return null;
                    }
                }
                con.Close();
            }
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            return null;
        }
    }

    public User CheckLogin(AuthenticateRequest request)
    {
        try
        {
            User user = new User();
            using (MySqlConnection con = GetConnection())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user where username = " + "\"" + request.Username + "\""
                                                                    + "and password = " + "\"" + request.Password + "\"", con);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"].ToString());
                            string username = reader["username"].ToString();
                            string password = reader["password"].ToString();
                            string FirstName = reader["firstname"].ToString();
                            string LastName = reader["lastname"].ToString();
                            user = new User(id, FirstName, LastName, username, password);
                        }
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        con.Close();
                        return null;
                    }
                }
                con.Close();
            }
            return user;
        }
        catch (Exception err)
        {
            Console.WriteLine(err.ToString());
            return null;
        }
    }
    public void Create(RegisterRequest request)
    {
        try
        {
            User user = new User();
            using (MySqlConnection con = GetConnection())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("insert into user(username, password, firstname, lastname)"
                                                    + "values('" + request.Username + "','" + request.Password + "','"
                                                    + request.FirstName + "','" + request.LastName + "');", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                    }
                    reader.Close();
                }
                con.Close();
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err);
        }
    }

}