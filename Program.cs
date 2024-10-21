using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
<<<<<<< HEAD
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
=======
using System.Xml.Serialization;

>>>>>>> PointsMethods

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();

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
<<<<<<< HEAD
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Add authorization policies if needed
// builder.Services.AddAuthorizationPolicies();
=======
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: builder.Configuration["Jwt:Key"]))
        }; 
    });

// Add authorization policies
builder.Services.AddAuthorizationPolicies();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<EventService>();
builder.Services.AddSingleton<IPointSystemService, PointSystemService>();
>>>>>>> PointsMethods

var app = builder.Build();
app.Urls.Add("http://localhost:5001");
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "This is the home page");

app.MapControllers();

app.Run();
<<<<<<< HEAD

public record User(Guid Id, string Firstname, string Lastname, string Email, string Password, int RecuringDays, UserRole Role);

public record EventAttendance(Guid Id, int UserID, int EventID, int Rating, string Feedback); // Recheck what Rating is.
public record Event(Guid Id, string Title, string Description, DateTime StartTime, DateTime EndTime, string Location, bool Approval)
{
    internal readonly Guid id;
}

public record Attendance(int UserID, DateTime Date) // Add user id to this attendance
{
    internal readonly Guid UserId;
}

public record Admin(Guid Id, string Username, string Password, string Email);
=======
>>>>>>> PointsMethods
