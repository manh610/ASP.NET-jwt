using WebApi.Helpers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;
    services.AddCors();
    services.AddControllers();

    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    services.AddSingleton<IUserService, UserService>();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddMvc();

    services.Configure<RouteOptions> ( option => {
        option.LowercaseQueryStrings = true;
        option.LowercaseUrls = true;
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

{
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.Run();
