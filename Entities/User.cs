namespace WebApi.Entities;

using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }

    public List<Role> Role {get; set;}

    public List<OrgRole> orgRole {get; set;}

    [JsonIgnore]
    public string Password { get; set; }

    public User() {

    }

    public User(int id, string firstname, string lastname, string username, string password) {
        this.Id = id;
        this.FirstName = firstname;
        this.LastName = lastname;
        this.Username = username;
        this.Password = password;
    }

    public string toString() {
        return "{\"Id\":" + "\""+Id.ToString() + "\"" + 
                "," + "\"FirstName\":" + "\"" + FirstName + "\"" + 
                "," + "\"LastName\":" + "\"" + LastName + "\"" +
                "," + "\"Username\":" + "\"" + Username + "\"}";
    }
}