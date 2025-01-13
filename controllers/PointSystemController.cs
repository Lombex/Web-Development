using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

    [HttpGet("{userId}/history")]
    public async Task<IActionResult> GetPointsHistory(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("User not found");
        
        var history = await _pointSystemService.GetPointsHistory(userId);
        return Ok(history);
    }

    [HttpGet("{userId}/level")]
    public async Task<IActionResult> GetUserLevel(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("User not found");
        
        var level = await _pointSystemService.GetUserLevel(user);
        return Ok(level);
    }

    [HttpGet("{userId}/streak")]
    public async Task<IActionResult> GetUserStreak(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("User not found");

        var streak = await _pointSystemService.UpdateStreak(user);
        return Ok(streak);
    }

    [HttpPost("{userId}/purchase")]
    public async Task<IActionResult> PurchaseItem(Guid userId, [FromBody] ShopItems item)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("User not found");

        if (user.Points.Items.Any(i => i.Name == item.Name))
        {
            return BadRequest("Item already owned");
        }

        var result = await _pointSystemService.BuyItem(user, item);
        if (!result) return BadRequest("Insufficient points");

        await _userService.UpdateUserAsync(userId, user);
        return Ok("Item purchased successfully");
    }

    // 1. Get User Points
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserPoints(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("user does not exist!");
        return Ok(user.Points.PointAmount);
    }

    // 2. Add Points to User
    [HttpPost("{userId}/add")]
    public async Task<IActionResult> AddPoints(Guid userId, [FromBody] AddPointsRequest request)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("User does not exist!");

        await _pointSystemService.AddUserPoints(user, request.Amount, request.Reason ?? "Points added");
        await _userService.UpdateUserAsync(userId, user);

        return Ok($"Added {request.Amount} points to user {user.Firstname} {user.Lastname}.");
    }

    public class AddPointsRequest
    {
        public int Amount { get; set; }
        public string? Reason { get; set; }
    }

    // 3. Update User Points
    [HttpPut("{userId}/update")]
    public async Task<IActionResult> UpdateUserPoints(Guid userId, [FromBody] int amount)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("user does not exist!");
        
        var result = await _pointSystemService.UpdateUserPoints(user, amount);
        return result ? Ok() : NotFound();
    }


}
