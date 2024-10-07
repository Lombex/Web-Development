using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase 
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("Test")]
    public IActionResult APIHealth() => Ok("API is healthy!");

    [Authorize(Policy = Policies.RequireUserRole)]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new { 
            Message = "This is accessible to all authenticated users", 
            UserId = userId,
            Email = userEmail,
            Role = userRole
        });
    }

    //[Authorize(Policy = Policies.RequireAdminRole)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        var _user = new User(Guid.NewGuid(), user.Firstname, user.Lastname, user.Email, user.Password, user.RecuringDays, user.Role);
        await _userService.CreateUserAsync(_user);
        return Ok("User has been successfully created!");
    }

    [Authorize(Policy = Policies.RequireAdminRole)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var _user = await _userService.GetUserAsync(id);
        if (_user == null)
            return NotFound("User not found");
        return Ok(_user);
    }

    [Authorize(Policy = Policies.RequireAdminRole)]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [Authorize(Policy = Policies.RequireAdminRole)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        var updatedUser = await _userService.UpdateUserAsync(user.id, user);
        if (updatedUser == null)
            return NotFound("User not found");
        return Ok("User has been successfully updated");
    }

    [Authorize(Policy = Policies.RequireAdminRole)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
            return NotFound("User not found");
        return Ok("User has been successfully deleted!");
    }
}