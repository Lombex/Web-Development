using Microsoft.AspNetCore.Mvc;
using static PointSystem;

[Route("api/points")]
public class PointSystemController : ControllerBase
{
    [HttpGet("Test")] // http://localhost:5001/api/points/Test
    public IActionResult APIHealth() => Ok("API is healthy");

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPoints([FromQuery] PointRecord pointRecord)
    {
        var _user = new User(pointRecord.user.id, pointRecord.user.Firstname, pointRecord.user.Lastname, pointRecord.user.Email, pointRecord.user.Password, pointRecord.user.RecuringDays);
        
        return Ok("");
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddPoints(PointRecord pointRecord)
    {
        var _user = new User(pointRecord.user.id, pointRecord.user.Firstname, pointRecord.user.Lastname, pointRecord.user.Email, pointRecord.user.Password, pointRecord.user.RecuringDays);

        return Ok("");
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> RemovePoints([FromQuery] PointRecord pointRecord)
    {
        var _user = new User(pointRecord.user.id, pointRecord.user.Firstname, pointRecord.user.Lastname, pointRecord.user.Email, pointRecord.user.Password, pointRecord.user.RecuringDays);

        return Ok("");
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdatePoints([FromQuery] PointRecord pointRecord)
    {
        var _user = new User(pointRecord.user.id, pointRecord.user.Firstname, pointRecord.user.Lastname, pointRecord.user.Email, pointRecord.user.Password, pointRecord.user.RecuringDays);

        return Ok("");
    }

}