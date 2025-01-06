using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    [HttpGet("Test")] // http://localhost:5001/api/events/Test
    public IActionResult APIHealth() => Ok("Event API is healthy!");
    [HttpPost("create")] // http://localhost:5001/api/events/create
    public async Task<IActionResult> CreateEvent([FromBody] Event eventItem)
    {
        var _event = new Event(Guid.NewGuid(), eventItem.Title, eventItem.Description, eventItem.StartTime, eventItem.EndTime, eventItem.Location, eventItem.Approval);
        // Save "_event" to new database. 
        return Ok($"Event has been successfully created! ID:{_event.Id} Title:{_event.Title}");
    }
    [HttpGet("{id}")] // http://localhost:5001/api/events/{id}
    public async Task<IActionResult> GetEvent(Guid id)
    {
        // In a real scenario, you'd fetch the event from the database using the id
        var _event = new Event(id, "Sample Event", "Description", DateTime.Now, DateTime.Now.AddHours(2), "Sample Location", true);
        // Get "_event" info from database. 
        return Ok(_event);
    }
    [HttpGet("all")] // http://localhost:5001/api/events/all
    public async Task<IActionResult> GetAllEvents()
    {
        // In a real scenario, you'd fetch all events from the database
        var _event = new Event(Guid.NewGuid(), "Sample Event", "Description", DateTime.Now, DateTime.Now.AddHours(2), "Sample Location", true);
        // Get all events from database. 
        return Ok(new[] { _event });
    }
    [HttpPut("update")] // http://localhost:5001/api/events/update
    public async Task<IActionResult> UpdateEvent([FromBody] Event eventItem)
    {
        var _event = new Event(eventItem.Id, eventItem.Title, eventItem.Description, eventItem.StartTime, eventItem.EndTime, eventItem.Location, eventItem.Approval);
        // Update entire Event from "_event" in the database
        return Ok("Event has been successfully updated");
    }
    [HttpDelete("delete")] // http://localhost:5001/api/events/delete
    public async Task<IActionResult> DeleteEvent([FromBody] Event eventItem)
    {
        var _event = new Event(eventItem.Id, eventItem.Title, eventItem.Description, eventItem.StartTime, eventItem.EndTime, eventItem.Location, eventItem.Approval);
        // Delete "_event" from database.
        return Ok("Event has been successfully deleted!");
    }
}