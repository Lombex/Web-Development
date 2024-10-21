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

        var result = await _eventattendanceService.RegisterEventAttendance(eventAttendance);
        if (!result.IsSuccess) return StatusCode(500, result.ErrorMessage);

        return Ok(result.Message);
    }

    [Authorize(Policy = Policies.RequireUserRole)]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetEventAttendances(Guid userId)
    {
        var attendances = await _eventattendanceService.GetEventAttendances(userId);
        if (attendances == null || attendances.Count == 0) return NotFound($"No attendances found for User ID {userId}");
        
        return Ok(attendances);
    }

    [Authorize(Policy = Policies.RequireAdminRole)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteEventAttendance(Guid id)
    {
        var result = await _eventattendanceService.RemoveEventAttendance(id);
        if (!result.IsSuccess) return NotFound(result.ErrorMessage);

        return Ok($"EventAttendance with ID {id} has been successfully deleted");
    }
}
