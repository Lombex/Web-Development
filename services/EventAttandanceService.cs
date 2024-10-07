using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IEventAttendanceService
{
    Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterEventAttendance(EventAttendance attendance);
    Task<List<EventAttendance>> GetEventAttendances(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> RemoveEventAttendance(Guid id);
}

public class EventAttendanceService : IEventAttendanceService
{
    private readonly List<EventAttendance> _eventAttendances = new();

    public async Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterEventAttendance(EventAttendance attendance)
    {
        try
        {
            _eventAttendances.Add(attendance);
            return await Task.FromResult<(bool, string, string)>((true, "Event attendance registered successfully", null));
        }
        catch (Exception ex)
        {
            return await Task.FromResult<(bool, string, string)>((false, null, $"Error: {ex.Message}"));
        }
    }

    public async Task<List<EventAttendance>> GetEventAttendances(Guid userId)
    {
        var userAttendances = _eventAttendances.Where(ea => ea.UserID == userId).ToList();
        return await Task.FromResult<List<EventAttendance>>(userAttendances);
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> RemoveEventAttendance(Guid id)
    {
        var attendance = _eventAttendances.FirstOrDefault(ea => ea.Id == id);
        if (attendance == null)
        {
            return await Task.FromResult<(bool, string)>((false, "Event attendance not found"));
        }

        _eventAttendances.Remove(attendance);
        return await Task.FromResult<(bool, string)>((true, null));
    }
}

// Record for EventAttendance

