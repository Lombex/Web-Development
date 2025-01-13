using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
public class FeedbackModel
{
    public int Rating { get; set; }
    public string Comment { get; set; }
}
[ApiController]
[Route("api/eventattendance")]
public class EventAttendanceController : ControllerBase
{
    private readonly IEventAttendanceService _eventattendanceService;
    private readonly IPointSystemService _pointSystemService;
    private readonly IUserService _userService;
    private readonly IEventService _eventService;

    public EventAttendanceController(
        IEventAttendanceService eventattendanceService,
        IPointSystemService pointSystemService,
        IUserService userService,
        IEventService eventService)
    {
        _eventattendanceService = eventattendanceService;
        _pointSystemService = pointSystemService;
        _userService = userService;
        _eventService = eventService;
    }

    [HttpPost("{attendanceId}/feedback")]
    public async Task<IActionResult> ProvideFeedback(Guid attendanceId, [FromBody] FeedbackModel feedback)
    {
        var attendance = await _eventattendanceService.GetEventAttendanceAsync(attendanceId);
        if (attendance == null) return NotFound("Attendance record not found");

        attendance.Rating = feedback.Rating;
        attendance.Feedback = feedback.Comment;
        attendance.FeedbackProvided = true;

        var user = await _userService.GetUserAsync(attendance.UserId);
        var @event = await _eventService.GetEventAsync(attendance.EventId);

        if (user != null && @event != null)
        {
            await _pointSystemService.AddEventPoints(user, @event, true);
        }

        var result = await _eventattendanceService.UpdateEventAttendanceAsync(attendance);
        return Ok("Feedback recorded and points awarded");
    }

    [HttpGet("Test")]
    public IActionResult APIHealth() => Ok("EventAttendance API is healthy!");

    // [Authorize(Policy = Policies.RequireUserRole)] // Verwijderd voor testdoeleinden
    [HttpPost("create")]
    public async Task<IActionResult> CreateEventAttendance([FromBody] EventAttendance eventAttendance)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        eventAttendance.Id = Guid.NewGuid(); // Zorg ervoor dat de ID wordt gegenereerd
        var result = await _eventattendanceService.RegisterEventAttendance(eventAttendance);

        if (!result.IsSuccess) 
            return StatusCode(500, result.ErrorMessage ?? "Er is een fout opgetreden bij het registreren van de event attendance.");

        return Ok(result.Message);
    }

    // [Authorize(Policy = Policies.RequireUserRole)] // Verwijderd voor testdoeleinden
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetEventAttendances(Guid userId)
    {
        var attendances = await _eventattendanceService.GetEventAttendances(userId);
        if (attendances == null || attendances.Count == 0) 
            return NotFound($"Geen attendances gevonden voor User ID {userId}");
        
        return Ok(attendances);
    }

    // [Authorize(Policy = Policies.RequireAdminRole)] // Verwijderd voor testdoeleinden
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteEventAttendance(Guid id)
    {
        var result = await _eventattendanceService.RemoveEventAttendance(id);
        if (!result.IsSuccess) return NotFound(result.ErrorMessage);

        return Ok($"EventAttendance met ID {id} is succesvol verwijderd");
    }
}
