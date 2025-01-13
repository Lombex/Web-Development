using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IEventAttendanceService
{
    Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterEventAttendance(EventAttendance attendance);
    Task<List<EventAttendance>> GetEventAttendances(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> RemoveEventAttendance(Guid id);
    // New methods
    Task<EventAttendance> GetEventAttendanceAsync(Guid id);
    Task<bool> UpdateEventAttendanceAsync(EventAttendance attendance);
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

    public async Task<EventAttendance> GetEventAttendanceAsync(Guid id)
    {
        return await _context.EventAttendances
            .Include(ea => ea.User)
            .Include(ea => ea.Event)
            .FirstOrDefaultAsync(ea => ea.Id == id);
    }

    public async Task<bool> UpdateEventAttendanceAsync(EventAttendance attendance)
    {
        try
        {
            _context.EventAttendances.Update(attendance);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
