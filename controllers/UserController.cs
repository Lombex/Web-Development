using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

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

    [HttpGet("fromToken")]
    public async Task<ActionResult<User>> GetUserFromToken()
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Unauthorized(new { message = "Token is missing or invalid" });
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();

        try
        {
            var userId = _userService.GetUserIdFromToken(token);
            var user = await _userService.GetUserAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new
            {
                user.Id,
                user.Firstname,
                user.Lastname,
                user.Email,
                user.Role,
                Points = new
                {
                    user.Points.PointAmount,
                    user.Points.AllTimePoints,
                    Items = user.Points.Items
                }
            });
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }


    [HttpPut("me")]
    public async Task<IActionResult> UpdateUserDetails([FromBody] User updatedUser)
    {
        // Get the Authorization header and extract the token
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Unauthorized(new { message = "Token is missing or invalid" });
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();

        try
        {
            var userId = _userService.GetUserIdFromToken(token);

            var user = await _userService.GetUserAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            user.Firstname = updatedUser.Firstname;
            user.Lastname = updatedUser.Lastname;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;

            await _userService.UpdateUserAsync(userId, user);

            return Ok(new
            {
                message = "User updated successfully",
                user = new
                {
                    user.Id,
                    user.Firstname,
                    user.Lastname,
                    user.Email,
                    user.Points
                }
            });
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred", details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
