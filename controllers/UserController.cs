using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase {
    
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("Test")] // http://localhost:5001/api/user/Test
    public IActionResult APIHealth() => Ok("API is healthy!");

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        var _user = new User(Guid.NewGuid(), user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays, user.Role);
        await _userService.CreateUserAsync(_user);
        return Ok("User has been successfully created!");
    }

    [HttpGet("{id}")] // Get ID from user for url param | http://localhost:5001/api/user/{id}
    public async Task<IActionResult> GetUser(Guid id) {
        var _user = await _userService.GetUserAsync(id);
        // Get "_user" info from database. 
        if (_user == null)
            return NotFound("User not found");
        return Ok(_user);
    }

    [HttpGet("all")] // http://localhost:5001/api/user/all
    public async Task<IActionResult> GetAllUsers() {
        var users = await _userService.GetAllUsersAsync();
        // Get all users from database. 
        return Ok(users);
    }

    [HttpPut("update/v1")] // http:localhost:5001/api/user/update/v1
    public async Task<IActionResult> UpdateUser([FromBody] User user) {
        var updatedUser = await _userService.UpdateUserAsync(user.id, user);
        // Update Entire User from "_user" 
        if (updatedUser == null)
            return NotFound("User not found");
        return Ok("User has been successfully updated");
    }

    // [HttpPatch("update/v2")]// https:localhost:5000/api/user/update/v2
    // public async Task<IActionResult> UpdateUser([FromBody] User user) {
    //     var _user = new User(user.id, user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays);
    //     // Update Entire User from "_user" 
    //     return Ok("User has been successfully updated");
    // }

    [HttpDelete("delete")] // http:localhost:5001/api/user/delete
    public async Task<IActionResult> DeleteUser([FromBody] User user) {
        var result = await _userService.DeleteUserAsync(user.id);
        // Delete "_user" from database.
        if (!result)
            return NotFound("User not found");
        return Ok("User has been successfully deleted!");
    }
}