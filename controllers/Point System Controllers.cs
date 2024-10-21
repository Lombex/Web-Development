using Microsoft.AspNetCore.Mvc;

[Route("api/points")]
public class PointSystemController : ControllerBase
{
    private readonly IPointSystemService _pointService;

    public PointSystemController(IPointSystemService pointService)
    {
        _pointService = pointService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPoints(Guid userId)
    {
        var user = await _pointService.GetPointsFromUser(new User { id = userId });
        return Ok(user);
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddPoints([FromBody] UserPointsModel pointRecord, int amount)
    {
        _pointService.AddUserPoints(pointRecord.user, amount);
        return Ok("Points added");
    }

    [HttpPut("Update/{userId}")]
    public async Task<IActionResult> UpdatePoints(Guid userId, int amount)
    {
        var result = await _pointService.UpdateUserPoints(new User { id = userId }, amount);
        return result ? Ok("Points updated") : BadRequest("Failed to update points");
    }

    [HttpDelete("Delete/{userId}")]
    public async Task<IActionResult> RemovePoints(Guid userId, int amount)
    {
        var result = await _pointService.RemoveUserPoints(new User { id = userId }, amount);
        return result ? Ok("Points removed") : BadRequest("Failed to remove points");
    }
}