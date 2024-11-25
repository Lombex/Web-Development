using Microsoft.AspNetCore.Mvc;

[Route("api/points")]
[ApiController]
public class PointSystemController : ControllerBase
{
    private readonly IPointSystemService _pointSystemService;

    public PointSystemController(IPointSystemService pointSystemService)
    {
        _pointSystemService = pointSystemService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserPoints(Guid userId)
    {
        var points = await _pointSystemService.GetUserPointAmount(userId);
        return Ok(points);
    }

    [HttpPost("{userId}/add")]
    public async Task<IActionResult> AddPointsToUser(Guid userId, [FromBody] int amount)
    {
        await _pointSystemService.AddPointsToUser(userId, amount);
        return Ok(new { message = "Points added successfully!" });
    }

    [HttpPut("{userId}/update")]
    public async Task<IActionResult> UpdateUserPoints(Guid userId, [FromBody] int amount)
    {
        await _pointSystemService.UpdateUserPoint(userId, amount);
        return Ok(new { message = "Points updated successfully!" });
    }

}
