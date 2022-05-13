namespace WebApi.Services;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Entities;
using WebApi.DAO;
using WebApi.Models;
using WebApi.Helpers;
public interface IUserService
{
    Response Authenticate(AuthenticateRequest model);
    IEnumerable<AuthenticateResponse> GetAll();
    Response GetById(int id);
    Response Register(RegisterRequest request);
}

public class UserService : IUserService
{
    private List<User> _users = new List<User>();

    private readonly AppSettings _appSettings;

    private UserContext userContext;
    private OrgRoleContext orgRoleContext;
    private SystemRoleContext systemRoleContext;
    private OrgRoleUserContext orgRoleUserContext;
    private SystemRoleUserContext systemRoleUserContext;

    public UserService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        userContext = new UserContext();
        orgRoleContext = new OrgRoleContext();
        systemRoleContext = new SystemRoleContext();
        orgRoleUserContext = new OrgRoleUserContext();
        systemRoleUserContext = new SystemRoleUserContext();
    }


    public Response Authenticate(AuthenticateRequest model)
    {

        Response response = new Response();

        User user = userContext.CheckLogin(model);

        if (user == null)
        {
            response.message = "Username hoac password khong dung";
            response.data = new Object();
            return response;
        } else {
            user.OrgRole = GetOrgRolesByUser(user);
            user.SystemRole = GetSystemRolesByUser(user);
            var token = generateJwtToken(user);
            response.data = new AuthenticateResponse(user, token);
            response.message = "Dang nhap thanh cong";
            return response;
        }
    }

    public IEnumerable<AuthenticateResponse> GetAll()
    {
        List<AuthenticateResponse> responses = new List<AuthenticateResponse>();
        List<User> listUser = userContext.GetAllUser();
        foreach (User user in listUser)
        {
            user.OrgRole = GetOrgRolesByUser(user);
            user.SystemRole = GetSystemRolesByUser(user);
            var token = generateJwtToken(user);
            responses.Add(new AuthenticateResponse(user, token));
        }
        return responses;
    }

    private List<String> GetOrgRolesByUser(User user)
    {
        List<String> listOrgRole = new List<String>();
        List<int> listId = orgRoleUserContext.GetOrgRoleByUserId(user.Id);
        foreach (int i in listId)
        {
            OrgRole orgRole = orgRoleContext.GetOrgRoleById(i);
            listOrgRole.Add(orgRole.Role);
        }
        return listOrgRole;
    }
    private List<String> GetSystemRolesByUser(User user)
    {
        List<String> listOrgRole = new List<String>();
        List<int> listId = systemRoleUserContext.GetSystemRoleByUserId(user.Id);
        foreach (int i in listId)
        {
            SystemRole systemRole = systemRoleContext.GetSystemRoleById(i);
            listOrgRole.Add(systemRole.Role);
        }
        return listOrgRole;
    }

    public Response GetById(int id)
    {
        Response response = new Response();
        User user = userContext.GetUserById(id);
        if (user == null)
        {
            response.data = new Object();
            response.message = "Khong tim thay user";
            return response;
        }
        user.OrgRole = GetOrgRolesByUser(user);
        user.SystemRole = GetSystemRolesByUser(user);
        var token = generateJwtToken(user);
        response.data = new AuthenticateResponse(user, token);
        response.message = "OK";
        return response;
    }


    private string generateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("profile", user.ToString()), 
                                                new Claim("OrgRoles",string.Join(", ", user.OrgRole)),
                                                new Claim("SysRoles",string.Join(", ", user.SystemRole))}),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public Response Register(RegisterRequest request)
    {
        Response response = new Response();
        if (request.Password != request.ConfirmPassword)
        {
            response.data = new Object();
            response.message = "Mat khau khong khop voi xac nhan mat khau";
            return response;
        }
        User user = userContext.GetUserByUsername(request.Username);
        if (user != null)
        {
            response.data = new Object();
            response.message = "Username da ton tai";
            return response;
        }
        userContext.Create(request);
        User newUser = userContext.GetUserByUsername(request.Username);
        newUser.OrgRole = GetOrgRolesByUser(newUser);
        newUser.SystemRole = GetSystemRolesByUser(newUser);
        var token = generateJwtToken(newUser);
        response.data = new AuthenticateResponse(newUser, token);
        response.message = "Dang ki thanh cong";
        return response;
    }

}

