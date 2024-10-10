using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Add authorization policies if needed
// builder.Services.AddAuthorizationPolicies();

var app = builder.Build();
app.Urls.Add("http://localhost:5001");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "This is the home page");

app.MapControllers();

app.Run();

public record User(Guid Id, string Firstname, string Lastname, string Email, string Password, int RecuringDays, UserRole Role)
{
    internal readonly Guid id;
}
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
