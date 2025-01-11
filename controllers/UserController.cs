using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route("api/user")]
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
        try
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        return Ok(await _userService.GetAllUsersAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet("me")]
    public async Task<ActionResult<User>> GetUserDetails()
    {
        var email = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(email))
            return Unauthorized(new {  message = "Invalid token" });

        var user = await _userService.GetUserByEmailAsync(email);

        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }



    [HttpPut("me")]
    public async Task<IActionResult> UpdateUserDetails([FromBody] User updatedUser)
    {
        var email = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(email))
            return Unauthorized(new { message = "Invalid token" });

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return NotFound(new { message = "User not found" });

        user.Firstname = updatedUser.Firstname;
        user.Lastname = updatedUser.Lastname;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;

        await _userService.UpdateUserAsync(user.Id, user);

        return Ok(user);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
