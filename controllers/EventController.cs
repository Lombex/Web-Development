using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IPointSystemService _pointSystemService;
    private readonly IUserService _userService;

    public EventController(
        IEventService eventService, 
        IPointSystemService pointSystemService,
        IUserService userService)
    {
        _eventService = eventService;
        _pointSystemService = pointSystemService;
        _userService = userService;
    }

    [HttpPost("complete/{eventId}")]
    public async Task<IActionResult> CompleteEvent(Guid eventId)
    {
        var @event = await _eventService.GetEventAsync(eventId);
        if (@event == null) return NotFound("Event not found");

        // Update event completion status and award points to participants
        var updatedEvent = await _eventService.CompleteEventAsync(eventId);
        if (updatedEvent == null) return NotFound("Event not found");

        // Award points to all participants
        foreach (var attendance in updatedEvent.EventAttendances)
        {
            var user = await _userService.GetUserAsync(attendance.UserId);
            if (user != null)
            {
                await _pointSystemService.AddEventPoints(user, updatedEvent, attendance.FeedbackProvided);
            }
        }

        return Ok("Event completed and points awarded");
    }

    [HttpGet("Test")] // http://localhost:5001/api/events/Test
    public IActionResult APIHealth() => Ok("Event API is healthy!");

    [HttpPost("create")] // http://localhost:5001/api/events/create
    public async Task<IActionResult> CreateEvent([FromBody] Event eventItem)
    {
        var createdEvent = await _eventService.CreateEventAsync(new Event(Guid.NewGuid(), eventItem.Title, eventItem.Description, eventItem.StartTime, eventItem.EndTime, eventItem.Location, eventItem.Approval));
        return Ok($"Event has been successfully created! ID:{createdEvent.Id} Title:{createdEvent.Title}");
    }

    [HttpGet("{id}")] // http://localhost:5001/api/events/{id}
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var _event = await _eventService.GetEventAsync(id); // Haal event op vanuit database
        if (_event == null) return NotFound("Event not found.");
        return Ok(_event);
    }

    [HttpGet("all")] // http://localhost:5001/api/events/all
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync(); // Haal alle events uit de database
        return Ok(events);
    }

    [HttpPut("update")] // http://localhost:5001/api/events/update
    public async Task<IActionResult> UpdateEvent([FromBody] Event eventItem)
    {
        var updatedEvent = await _eventService.UpdateEventAsync(eventItem.Id, eventItem);
        if (updatedEvent == null) return NotFound("Event not found.");
        return Ok("Event has been successfully updated");
    }

    [HttpDelete("delete/{id}")] // http://localhost:5001/api/events/delete/{id}
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var isDeleted = await _eventService.DeleteEventAsync(id);
        if (!isDeleted) return NotFound("Event not found.");
        return Ok("Event has been successfully deleted!");
    }
}
