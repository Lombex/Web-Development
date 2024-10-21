<<<<<<< HEAD
// UserController.cs
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
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
        if (user == null)
        {
            return BadRequest("User data is invalid.");
        }

        try
        {
            var newUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User updatedUser)
    {
        if (updatedUser == null)
        {
            return BadRequest("User data is invalid.");
        }

        try
        {
            var user = await _userService.UpdateUserAsync(id, updatedUser);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

     [HttpDelete("{id}")] // Delete user
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            if (!isDeleted)
            {
                return NotFound("User not found.");
            }
            return Ok("User deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
=======
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
     private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpGet("Test")]
    public IActionResult Test()
    {
        return Ok("API is running!");
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        var createdUser = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id =  Guid.NewGuid() }, createdUser);
    }

    [HttpGet("all")]
    //[Authorize(Policies.RequireUserRole)]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        return Ok(await _userService.GetAllUsersAsync());
    }

    [HttpGet("{id}")]
    //[Authorize(Policies.RequireUserRole)]
    public async Task<ActionResult<User>> GetUserById(Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPut("{id}")]
    //[Authorize(Policies.RequireUserRole)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
    {
        var updatedUser = await _userService.UpdateUserAsync(id, user);
        if (updatedUser == null) return NotFound();
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    //[Authorize(Policies.RequireAdminRole)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
>>>>>>> PointsMethods
}
