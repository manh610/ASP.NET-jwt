namespace WebApi.Services;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models;

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

    public UserService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        userContext = new UserContext();
    }


    public Response Authenticate(AuthenticateRequest model)
    {

        Response response = new Response();

        var user = userContext.CheckLogin(model);

        if (user == null)
        {
            response.message = "Username hoac password khong dung";
            response.data = new Object();
            return response;
        }
        var token = generateJwtToken(user);
        response.data = new AuthenticateResponse(user, token);
        response.message = "Dang nhap thanh cong";
        return response;
    }

    public IEnumerable<AuthenticateResponse> GetAll()
    {
        List<AuthenticateResponse> responses = new List<AuthenticateResponse>();
        List<User> listUser = userContext.GetAllUser();
        foreach (User user in listUser)
        {
            var token = generateJwtToken(user);

            responses.Add(new AuthenticateResponse(user, token));
        }
        return responses;
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
            Subject = new ClaimsIdentity(new[] { new Claim("profile", user.toString()) }),
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
        var token = generateJwtToken(newUser);
        response.data = new AuthenticateResponse(newUser, token);
        response.message = "Dang ki thanh cong";
        return response;
    }

}

