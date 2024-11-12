using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore; // Ensure this is included for DbContext
using System.Text;

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

// Add Authorization Policy
builder.Services.AddAuthorization(options =>
{
    // Ensure the User and Admin roles are properly defined
    options.AddPolicy("RequireUserRole", policy =>
        policy.RequireRole("User"));
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));
});

// Add DbContext for SQLite (ensure you have the connection string in appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register your services with Scoped lifetime, which is better for working with DbContext
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPointSystemService, PointSystemService>();
builder.Services.AddScoped<IEventAttendanceService, EventAttendanceService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>(); 

var app = builder.Build();

app.Urls.Add("http://localhost:5001");

// Middleware to ensure authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "This is the home page");

app.MapControllers();

app.Run();
