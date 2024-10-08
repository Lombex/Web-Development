
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });;

builder.Services.AddAuthorizationPolicies();
builder.Services.AddSingleton<IUserService, UserService>();

// Add authorization policies
builder.Services.AddAuthorizationPolicies();

var app = builder.Build();
app.Urls.Add("http://localhost:5001");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "This is the home page");

app.MapControllers();

app.Run();

public record User(Guid id, string Firstname, string Lastname, string Email, string Password, int RecuringDays, UserRole Role);
public record EventAttendance(Guid id, Guid UserId, Guid EventId, int Rating, string Feedback); // Recheck what Rating is.
public record Event(Guid id, string Title, string Description, DateTime StartTime, DateTime EndTime, string Location, bool Approval);
public record Attendance(Guid UserId, DateTime date); // Add user id to this attendace
public record Admin(Guid id, string Username, string Password, string Email);

