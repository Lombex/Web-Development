using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();
app.Urls.Add("http://localhost:5000");

app.MapGet("/", () => "This is the home page");

app.MapControllers();

app.Run();

public record EventAttendance(Guid id, int UserID, int EventID, int Rating, string Feedback); // Recheck what Rating is.
public record Event(Guid id, string Title, string Description, DateTime StartTime, DateTime EndTime, string Location, bool Approval);
public record Attendace(int UserID, DateTime date); // Add user id to this attendace
public record Admin(Guid id, string Username, string Password, string Email);

#region User
public record User(Guid id, string Firstname, string Lastname, string Email, string Password, int RecuringDays);

[Route("api/user")]
public class UserController : ControllerBase {
    
    
    [HttpGet("Test")] // https://localhost:5000/api/user/Test
    public async Task<IActionResult> APIHealth() => Ok("API is healthy!");

    [HttpPost("create")] // https://localhost:5000/api/user/create
    public async Task<IActionResult> CreateUser([FromBody] User user) {
        var _user = new User(Guid.NewGuid(), user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays);
        // Save "_user" to new database. 
        return Ok("User has been sucessfully created!");
    }

    [HttpGet()] // Get ID form user for url parm | https://localhost:5000/api/user/{id}
    public async Task<IActionResult> GetUser([FromBody] User user) {
        var _user = new User(Guid.NewGuid(), user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays);
        // Get "_user" info from database. 
        return Ok("return user here...");
    }

    [HttpGet("all")] // https://localhost:5000/api/user/all
    public async Task<IActionResult> GetAllUsers([FromBody] User user) {
        var _user = new User(Guid.NewGuid(), user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays);
        // Get all users from database. 
        return Ok("return user here...");
    }

    [HttpPut("update/v1")] // https:localhost:5000/api/user/update/v1
    public async Task<IActionResult> UpdateUser([FromBody] User user) {
        var _user = new User(user.id, user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays);
        // Update Intire User from "_user" 
        return Ok("User has been successfully updated");
    }

    // [HttpPatch("update/v2")]// https:localhost:5000/api/user/update/v2
    // public async Task<IActionResult> UpdateUser([FromBody] User user) {
    //     var _user = new User(user.id, user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays);
    //     // Update Intire User from "_user" 
    //     return Ok("User has been successfully updated");
    // }

    [HttpDelete("delete")] // https:localhost:5000/api/user/delete
    public async Task<IActionResult> DeleteUser([FromBody] User user) {
        var _user = new User(user.id, user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays);
        // Delete "_user" from database.
        return Ok("User has been succesfully deleted!");
    }
}

#endregion User

public interface IUserService
{
    Task<User?> GetUserAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(Guid id, User user);
    Task<bool> DeleteUserAsync(Guid id);
}
public class UserService : IUserService
{
    private readonly List<User> _users = new();

    public async Task<User?> GetUserAsync(Guid id)
    {
        return await Task.FromResult(_users.FirstOrDefault(u => u.id == id));
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await Task.FromResult(_users);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _users.Add(user);
        return await Task.FromResult(user);
    }

    public async Task<User?> UpdateUserAsync(Guid id, User user)
    {
        var index = _users.FindIndex(u => u.id == id);
        if (index == -1) return null;
        
        _users[index] = user with { id = id }; // Update met behoud van de originele ID
        return await Task.FromResult(_users[index]);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.id == id);
        if (user == null) return false;
        
        _users.Remove(user);
        return await Task.FromResult(true);
    }
}