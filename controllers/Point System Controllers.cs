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
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserPoints(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("user does not exist!");
        return Ok(user.Points.PointAmount);
    }

    // 2. Add Points to User
    [HttpPost("{userId}/add")]
    public async Task<IActionResult> AddPoints(Guid userId, [FromBody] int amount)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("User does not exist!");

        _pointSystemService.AddUserPoints(user, amount);
        await _userService.UpdateUserAsync(userId, user);
        
        return Ok($"Added {amount} points to user {user.Firstname} {user.Lastname}.");
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

    // 4. Get User Level
    [HttpGet("{userId}/level")]
    public async Task<IActionResult> GetUserLevel(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        if (user == null) return NotFound("user does not exist!");
        var level = await _pointSystemService.GetUserLevel(user);
        return Ok(level);
    }

    [HttpPost("{userId}/purchase")]
    public async Task<IActionResult> PurchaseItem(Guid userId, [FromBody] ShopItems item)
    {
        try
        {
            var user = await _userService.GetUserAsync(userId);
            if (user == null) return NotFound("User not found");
    
            // Check if user already owns this item
            if (user.Points.Items.Any(i => i.Name == item.Name))  // Just check by name for now
            {
                return BadRequest("Item already owned");
            }
    
            // Create new ShopItem with a new GUID since frontend sends string ID
            var shopItem = new ShopItems
            {
                Id = Guid.NewGuid(), // Generate new GUID for new items
                Name = item.Name,
                Description = item.Description,
                Price = item.Price
            };
    
            var purchaseResult = await _pointSystemService.BuyItem(user, shopItem);
            if (!purchaseResult)
            {
                return BadRequest("Not enough points");
            }
    
            await _userService.UpdateUserAsync(userId, user);
            return Ok("Item purchased successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error processing purchase: {ex.Message}");
        }
    }
}
