using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/eventattendance")]
public class EventAttendanceController : ControllerBase
{
    private readonly IEventAttendanceService _eventAttendanceService;
    private readonly IUserService _userService;
    private readonly IEventService _eventService;

    // Constructor with dependency injection
    public EventAttendanceController(IEventAttendanceService eventAttendanceService, IUserService userService, IEventService eventService)
    {
        _eventAttendanceService = eventAttendanceService;
        _userService = userService;
        _eventService = eventService;
    }

    // Test endpoint to check if the API is healthy
    [HttpGet("Test")]
    public IActionResult APIHealth() => Ok("EventAttendance API is healthy!");

    // Create new EventAttendance
    [HttpPost("create")]
    public async Task<IActionResult> CreateEventAttendance([FromBody] CreateEventAttendanceDTO dto)
    {
        // Validate the model
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Fetch the user and event from the database using the provided IDs
        var user = await _userService.GetUserAsync(dto.UserId);
        var eventItem = await _eventService.GetEventAsync(dto.EventId);

        if (user == null || eventItem == null)
        {
            return NotFound("User or Event not found");
        }

        // Create the EventAttendance entity with the full User and Event objects
        var eventAttendance = new EventAttendance
        {
            Id = Guid.NewGuid(),  
            User = user,          // Assign the fetched User object
            Event = eventItem,    // Assign the fetched Event object
            Rating = dto.Rating,
            Feedback = dto.Feedback
        };

        // Register the attendance using the service
        var result = await _eventAttendanceService.RegisterEventAttendance(eventAttendance);

        // Return response based on the result
        if (result.IsSuccess)
        {
            return Ok(result.Message); // Success response
        }
        else
        {
            return StatusCode(500, result.ErrorMessage ?? "An error occurred while registering the event attendance.");
        }
    }


    // Get all EventAttendances for a specific user
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetEventAttendances(Guid userId)
    {
        var attendances = await _eventAttendanceService.GetEventAttendances(userId);
        if (attendances == null || attendances.Count == 0)
            return NotFound($"No attendances found for User ID {userId}");

        return Ok(attendances);
    }

    // Delete an EventAttendance by Id
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteEventAttendance(Guid id)
    {
        var result = await _eventAttendanceService.RemoveEventAttendance(id);
        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok($"EventAttendance with ID {id} has been successfully deleted.");
    }
}
