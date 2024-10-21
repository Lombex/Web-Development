using Microsoft.AspNetCore.Mvc;

[Route("api/points")]
[ApiController]
public class PointSystemController : ControllerBase
{
    private readonly IPointSystemService _pointSystemService;
    private readonly IUserService _userService;

    public PointSystemController(IPointSystemService pointSystemService, IUserService userService)
    {
        _pointSystemService = pointSystemService;
        _userService = userService;
    }

    // 1. Get User Points
    [HttpGet("{userId}/points")]
    public async Task<IActionResult> GetUserPoints(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("user does not exits!");
        return Ok(user.Points.PointAmount);
    }

    // 2. Add Points to User
    [HttpPost("{userId}/points/add")]
    public async Task<IActionResult> AddPoints(Guid userId, [FromBody] int amount)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("user does not exits!");
        _pointSystemService.AddUserPoints(user, amount);
        return Ok();
    }

    // 3. Update User Points
    [HttpPut("{userId}/points/update")]
    public async Task<IActionResult> UpdateUserPoints(Guid userId, [FromBody] int amount)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("user does not exits!");
        var result = await _pointSystemService.UpdateUserPoints(user, amount);
        return result ? Ok() : NotFound();
    }

    /*// 4. Buy Item
    [HttpPost("{userId}/buy-item")]
    public async Task<IActionResult> BuyItem(Guid userId, [FromBody] Guid itemId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("user does not exits!");

    }*/

    /*// 5. Get All Shop Items
    [HttpGet("shop/items")]
    public async Task<IActionResult> GetAllShopItems()
    {
        var items = await _pointSystemService.GetAllShopItems();
        return Ok(items);
    }*/

    // 6. Get User Level
    [HttpGet("{userId}/level")]
    public async Task<IActionResult> GetUserLevel(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("user does not exits!");
        var level = await _pointSystemService.GetUserLevel(user);
        return Ok(level);
    }

    /*// 7. Get User's Items
    [HttpGet("{userId}/items")]
    public async Task<IActionResult> GetUserItems(Guid userId)
    {
        var items = await _pointSystemService.GetUserItems(userId);
        return Ok(items);
    }*/
}
