using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase {
    
    [HttpGet("Test")] // http://localhost:5001/api/user/Test
    public async Task<IActionResult> APIHealth() => Ok("API is healthy!");

    [HttpPost("create")] // http://localhost:5001/api/user/create
    public async Task<IActionResult> CreateUser([FromBody] User user) {
        var _user = new User(Guid.NewGuid(), user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays, user.Points);
        // Save "_user" to new database. 
        return Ok("User has been sucessfully created!");
    }

    [HttpGet()] // Get ID form user for url parm | http://localhost:5001/api/user/{id}
    public async Task<IActionResult> GetUser([FromBody] User user) {
        var _user = new User(Guid.NewGuid(), user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays, user.Points);
        // Get "_user" info from database. 
        return Ok("return user here...");
    }

    [HttpGet("all")] // http://localhost:5001/api/user/all
    public async Task<IActionResult> GetAllUsers([FromBody] User user) {
        var _user = new User(Guid.NewGuid(), user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays, user.Points);
        // Get all users from database. 
        return Ok("return user here...");
    }

    [HttpPut("update/v1")] // http:localhost:5001/api/user/update/v1
    public async Task<IActionResult> UpdateUser([FromBody] User user) {
        var _user = new User(user.id, user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays, user.Points);
        // Update Intire User from "_user" 
        return Ok("User has been successfully updated");
    }

    // [HttpPatch("update/v2")]// https:localhost:5000/api/user/update/v2
    // public async Task<IActionResult> UpdateUser([FromBody] User user) {
    //     var _user = new User(user.id, user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays);
    //     // Update Intire User from "_user" 
    //     return Ok("User has been successfully updated");
    // }

    [HttpDelete("delete")] // http:localhost:5001/api/user/delete
    public async Task<IActionResult> DeleteUser([FromBody] User user) {
        var _user = new User(user.id, user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays, user.Points);
        // Delete "_user" from database.
        return Ok("User has been succesfully deleted!");
    }
}