using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();
app.Urls.Add("https://localhost:5000");

app.MapGet("/", () => "This is the home page");

app.MapControllers();

app.Run();


public record User(Guid id, string Firstname, string Lastname, string Email, string Password, int RecuringDays);
public record EventAttendance(Guid id, int UserID, int EventID, int Rating, string Feedback); // Recheck what Rating is.
public record Event(Guid id, string Title, string Description, DateTime StartTime, DateTime EndTime, string Location, bool Approval);
public record Attandace(int UserID, DateTime date); // Add user id to this attendace
public record Admin(Guid id, string Username, string Password, string Email);


[Route("api/user")]
public class UserService : Controller {
    [HttpPost()] // https://localhost:5000/api/user
    public async Task<IActionResult> CreateUser([FromBody] User user) {
        var _user = new User(Guid.NewGuid(), user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays);
        // Save "_user" to new database. 
        return Ok("User has been sucessfully created!");
    }
}