using Microsoft.AspNetCore.Mvc;
using static PointSystem;

[Route("api/points")]
public class PointSystemController : ControllerBase
{
    [HttpGet("Test")] // http://localhost:5001/api/points/Test
    public IActionResult APIHealth() => Ok("API is healthy");

    [HttpPost("Add")]
    public async Task<IActionResult> AddPoints(PointRecord pointRecord)
    {
        if (pointRecord == null || pointRecord.user.id == Guid.Empty || pointRecord.Date == DateTime.MinValue) return BadRequest("Invalid PointRecord data.");
        var _user = new User(pointRecord.user.id, pointRecord.user.Firstname, pointRecord.user.Lastname, pointRecord.user.Email, pointRecord.user.Password, pointRecord.user.RecuringDays);
        return Ok("");
    }

    [HttpDelete("Remove")]
    public async Task<IActionResult> RemovePoints(PointRecord pointRecord)
    {
        var _user = new User(pointRecord.user.id, pointRecord.user.Firstname, pointRecord.user.Lastname, pointRecord.user.Email, pointRecord.user.Password, pointRecord.user.RecuringDays);
        return Ok("");
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdatePoints(PointRecord pointRecord)
    {
        var _user = new User(pointRecord.user.id, pointRecord.user.Firstname, pointRecord.user.Lastname, pointRecord.user.Email, pointRecord.user.Password, pointRecord.user.RecuringDays);
        return Ok("");
    }

}