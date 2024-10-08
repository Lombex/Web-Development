using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEventAttendanceService
{
    Task<EventAttendance?> GetEventAttendanceAsync(Guid id);
    Task<IEnumerable<EventAttendance>> GetAllEventAttendancesAsync();
    Task<EventAttendance> CreateEventAttendanceAsync(EventAttendance eventAttendance);
    Task<EventAttendance?> UpdateEventAttendanceAsync(Guid id, EventAttendance eventAttendance);
    Task<bool> DeleteEventAttendanceAsync(Guid id);
}

public class EventAttendanceService : IEventAttendanceService
{
    private readonly List<EventAttendance> _eventAttendances = new();

    // Get EventAttendance by ID
    public async Task<EventAttendance?> GetEventAttendanceAsync(Guid id)
    {
        return await Task.FromResult(_eventAttendances.FirstOrDefault(ea => ea.id == id));
    }

    // Get all EventAttendances
    public async Task<IEnumerable<EventAttendance>> GetAllEventAttendancesAsync()
    {
        return await Task.FromResult(_eventAttendances);
    }

    // Create a new EventAttendance
    public async Task<EventAttendance> CreateEventAttendanceAsync(EventAttendance eventAttendance)
    {
        _eventAttendances.Add(eventAttendance);
        return await Task.FromResult(eventAttendance);
    }

    // Update an existing EventAttendance
    public async Task<EventAttendance?> UpdateEventAttendanceAsync(Guid id, EventAttendance eventAttendance)
    {
        var existingAttendance = _eventAttendances.FirstOrDefault(ea => ea.id == id);
        if (existingAttendance == null)
        {
            return null; // Return null if the attendance entry is not found
        }

        // Update the entry
        existingAttendance = new EventAttendance(id, eventAttendance.UserId, eventAttendance.EventId, eventAttendance.Rating, eventAttendance.Feedback);
        return await Task.FromResult(existingAttendance);
    }

    // Delete EventAttendance
    public async Task<bool> DeleteEventAttendanceAsync(Guid id)
    {
        var attendance = _eventAttendances.FirstOrDefault(ea => ea.id == id);
        if (attendance == null) return false;

        _eventAttendances.Remove(attendance);
        return await Task.FromResult(true);
    }
}
