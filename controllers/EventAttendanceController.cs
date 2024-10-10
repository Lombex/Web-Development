using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/eventattendance")]

public class EventAttendanceController : ControllerBase
{
    private static List<EventAttendance> eventAttendances = new();
    [HttpGet("Test")] //http://localhost:5000/api/eventattendance/Test
    public IActionResult APIHealth() => Ok("EventAttendance API is healthy!");
  // Create a new EventAttendance entry
    [HttpPost("create")] // http://localhost:5001/api/eventattendance/create
    public async Task<IActionResult> CreateEventAttendance([FromBody] EventAttendance eventAttendance)
    {
        var newAttendance = new EventAttendance(Guid.NewGuid(), eventAttendance.UserID, eventAttendance.EventID, 
        eventAttendance.Rating, eventAttendance.Feedback
        );

        // Simulate saving to a database
        eventAttendances.Add(newAttendance);

        return Ok($"EventAttendance successfully created for Event ID: {newAttendance.EventID}");
    }

    // Get an EventAttendance entry by its ID
    [HttpGet("{id}")] // http://localhost:5000/api/eventattendance/{id}
    public async Task<IActionResult> GetEventAttendance(Guid id)
    {
        var attendance = eventAttendances.Find(ea => ea.Id == id);
        if (attendance == null)
        {
            return NotFound($"No attendance found with ID {id}");
        }

        return Ok(attendance);
    }

    // Get all EventAttendance entries
    [HttpGet("all")] // http://localhost:5001/api/eventattendance/all
    public async Task<IActionResult> GetAllEventAttendances()
    {
        return Ok(eventAttendances); // Return all attendance records
    }

    // Update an EventAttendance entry
    [HttpPut("update/{id}")] // http://localhost:5000/api/eventattendance/update/{id}
    public async Task<IActionResult> UpdateEventAttendance(Guid id, [FromBody] EventAttendance updatedAttendance)
    {
        var attendance = eventAttendances.Find(ea => ea.Id == id);
        if (attendance == null)
        {
            return NotFound($"No attendance found with ID {id}");
        }

        // Update the attendance details
        attendance = new EventAttendance(id, updatedAttendance.UserID, updatedAttendance.EventID, updatedAttendance.Rating, updatedAttendance.Feedback);
        return Ok($"EventAttendance with ID {id} has been successfully updated");
    }

    // Delete an EventAttendance entry
    [HttpDelete("delete/{id}")] // http://localhost:5000/api/eventattendance/delete/{id}
    public async Task<IActionResult> DeleteEventAttendance(Guid id)
    {
        var attendance = eventAttendances.Find(ea => ea.Id == id);
        if (attendance == null)
        {
            return NotFound($"No attendance found with ID {id}");
        }

        eventAttendances.Remove(attendance); // Simulate deletion
        return Ok($"EventAttendance with ID {id} has been successfully deleted");
    }
}
