using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/eventattendance")]
public class EventAttendanceController : ControllerBase
{
    private readonly IEventAttendanceService _eventAttendanceService;
    private readonly IUserService _userService;
    private readonly IEventService _eventService;

    public EventAttendanceController(IEventAttendanceService eventAttendanceService, IUserService userService, IEventService eventService)
    {
        _eventAttendanceService = eventAttendanceService;
        _userService = userService;
        _eventService = eventService;
    }

    [HttpGet("Test")]
    public IActionResult APIHealth() => Ok("EventAttendance API is healthy!");

    // Create new EventAttendance
    [HttpPost("create")]
    public async Task<IActionResult> CreateEventAttendance([FromBody] CreateEventAttendanceDTO dto)
    {
        // als het volgens de DTO niet klopt, dan geef een BadRequest terug
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // neemt de UserId en EventId van de DTO en haalt de User en Event op
        var user = await _userService.GetUserAsync(dto.UserId);
        var eventItem = await _eventService.GetEventAsync(dto.EventId);

        if (user == null || eventItem == null)
        {
            return NotFound("User or Event not found");
        }

        // maak een nieuwe EventAttendance object aan
        var eventAttendance = new EventAttendance
        {
            Id = Guid.NewGuid(),  
            User = user,         
            Event = eventItem,    
            Rating = dto.Rating,
            Feedback = dto.Feedback
        };

        // Register the attendance using the service
        var result = await _eventAttendanceService.RegisterEventAttendance(eventAttendance);
        if (result.IsSuccess)
        {
            return Ok(result.Message);
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
