using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/attendance")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpGet("Test")]
    public IActionResult APIHealth()
    {
        return Ok("Attendance API is healthy!");
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAttendance([FromBody] Attendance attendance)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        attendance.Id = Guid.NewGuid(); // Zorg ervoor dat een nieuwe ID wordt toegewezen
        var result = await _attendanceService.RegisterAttendance(attendance);

        if (!result.IsSuccess)
        {
            return StatusCode(500, result.ErrorMessage ?? "Er is iets misgegaan bij het registreren van de attendance.");
        }

        return Ok(result.Message);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAttendance(Guid id)
    {
        var result = await _attendanceService.RemoveAttendance(id);
        if (!result.IsSuccess)
        {
            return NotFound(result.ErrorMessage);
        }

        return Ok($"Attendance met ID {id} is succesvol verwijderd.");
    }

    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetAttendance(Guid id)
    {
        var Attendance = await _attendanceService.GetUserAttendances(id);
        if (Attendance == null) return BadRequest("This attendance does not exist!");
        return Ok(Attendance);
    }
}
