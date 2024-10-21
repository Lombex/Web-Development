using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

[ApiController]
[Route("api/eventattendance")]
public class EventAttendanceController : ControllerBase
{
    private readonly IEventAttendanceService _eventattendanceService;

    public EventAttendanceController(IEventAttendanceService eventattendanceService)
    {
        _eventattendanceService = eventattendanceService;
    }

    [HttpGet("Test")]
    public IActionResult APIHealth() => Ok("EventAttendance API is healthy!");

    [Authorize(Policy = Policies.RequireUserRole)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateEventAttendance([FromBody] EventAttendance eventAttendance)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Gebruik de juiste service voor event attendance
        var result = await _eventattendanceService.RegisterEventAttendance(eventAttendance);
        if (!result.IsSuccess) return StatusCode(500, result.ErrorMessage);

        return Ok(result.Message);
    }

    [Authorize(Policy = Policies.RequireUserRole)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventAttendance(Guid id)
    {
        // Gebruik de juiste service voor event attendance
        var attendance = await _eventattendanceService.GetEventAttendances(id);
        if (attendance == null || attendance.Count == 0) return NotFound($"No attendance found with ID {id}");
        
        return Ok(attendance);
    }

    [Authorize(Policy = Policies.RequireAdminRole)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteEventAttendance(Guid id)
    {
        // Gebruik de juiste service voor event attendance
        var result = await _eventattendanceService.RemoveEventAttendance(id);
        if (!result.IsSuccess) return NotFound(result.ErrorMessage);

        return Ok($"EventAttendance with ID {id} has been successfully deleted");
    }
}
