
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add authorization policies
builder.Services.AddAuthorizationPolicies();

var app = builder.Build();
app.Urls.Add("http://localhost:5001");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "This is the home page");

app.MapControllers();

app.Run();

public record User(Guid id, string Firstname, string Lastname, string Email, string Password, int RecuringDays);
public record EventAttendance(Guid Id, Guid UserID, Guid EventID, int Rating, string Feedback);
public record Event(Guid id, string Title, string Description, DateTime StartTime, DateTime EndTime, string Location, bool Approval);
public record Attendance(Guid UserID, DateTime date); // Add user id to this attendace
public record Admin(Guid id, string Username, string Password, string Email);
