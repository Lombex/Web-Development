using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IEventAttendanceService
{
    Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterEventAttendance(EventAttendance attendance);
    Task<List<EventAttendance>> GetEventAttendances(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> RemoveEventAttendance(Guid id);

    // Nieuwe methoden
    Task<EventAttendance> GetEventAttendanceAsync(Guid eventId, Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> UpdateEventAttendanceAsync(EventAttendance eventAttendance);
}

public class EventAttendanceService : IEventAttendanceService
{
    private readonly AppDbContext _context;

    public EventAttendanceService(AppDbContext context)
    {
        _context = context;
    }

    // Register een nieuwe event attendance
    public async Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterEventAttendance(EventAttendance attendance)
    {
        try
        {
            await _context.EventAttendances.AddAsync(attendance); // Voeg attendance toe aan de database
            await _context.SaveChangesAsync(); // Sla wijzigingen op
            return (true, "Event attendance successfully registered", null);
        }
        catch (Exception ex)
        {
            return (false, null, $"Error: {ex.Message}");
        }
    }

    // Haal alle attendances op voor een specifieke gebruiker
    public async Task<List<EventAttendance>> GetEventAttendances(Guid userId)
    {
        return await _context.EventAttendances
            .Where(ea => ea.UserId == userId) // Zoek op UserID in plaats van Id
            .ToListAsync();
    }

    // Verwijder een event attendance
    public async Task<(bool IsSuccess, string ErrorMessage)> RemoveEventAttendance(Guid id)
    {
        var attendance = await _context.EventAttendances.FindAsync(id); // Zoek de attendance op basis van ID
        if (attendance == null)
        {
            return (false, "Event attendance not found");
        }

        _context.EventAttendances.Remove(attendance); // Verwijder de attendance
        await _context.SaveChangesAsync(); // Sla wijzigingen op
        return (true, null);
    }
    public async Task<EventAttendance> GetEventAttendanceAsync(Guid eventId, Guid userId)
{
    // Zoek een specifieke event attendance op basis van EventId en UserId
    return await _context.EventAttendances
        .FirstOrDefaultAsync(ea => ea.EventId == eventId && ea.UserId == userId);
}

public async Task<(bool IsSuccess, string ErrorMessage)> UpdateEventAttendanceAsync(EventAttendance eventAttendance)
{
    try
    {
        _context.EventAttendances.Update(eventAttendance); // Update de attendance
        await _context.SaveChangesAsync(); // Sla de wijzigingen op
        return (true, null);
    }
    catch (Exception ex)
    {
        return (false, $"Error updating event attendance: {ex.Message}");
    }
}

}
