using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

[Route("api/users")]
[ApiController]
public class User_controller : ControllerBase
{
    private readonly IUserService _userService;

    public User_controller(IUserService userService)
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
        return CreatedAtAction(nameof(GetUserById), new { id = Guid.NewGuid() }, createdUser);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User user)
    {
        var updatedUser = await _userService.UpdateUserAsync(id, user);
        if (updatedUser == null) return NotFound();
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
