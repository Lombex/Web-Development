using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
    [HttpPost("review")]
    public async Task<IActionResult> AddReview([FromBody] ReviewRequest review)
    {
        var eventAttendance = await _eventattendanceService.GetEventAttendanceAsync(review.EventId, review.UserId);

        if (eventAttendance == null)
        {
            return NotFound("User is not associated with this event.");
        }

        // Update rating and feedback
        eventAttendance.Rating = review.Rating;
        eventAttendance.Feedback = review.Feedback;

        var result = await _eventattendanceService.UpdateEventAttendanceAsync(eventAttendance);

        if (!result.IsSuccess)
        {
            return StatusCode(500, result.ErrorMessage);
        }

        return Ok("Review added successfully.");
    }
}

// Klasse om de requestdata voor een review vast te leggen
public class ReviewRequest
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; }
    public string Feedback { get; set; }
}
