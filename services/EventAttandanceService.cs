using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IEventAttendanceService
{
    Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterEventAttendance(EventAttendance attendance);
    Task<List<EventAttendance>> GetEventAttendances(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> RemoveEventAttendance(Guid id);
}

public class EventAttendanceService : IEventAttendanceService
{
    private readonly AppDbContext _context;

    public EventAttendanceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterEventAttendance(EventAttendance attendance)
    {
        try
        {
            await _context.EventAttendances.AddAsync(attendance); // Voeg attendance toe aan de database
            await _context.SaveChangesAsync(); // Sla wijzigingen op
            return (true, "Event attendance registered successfully", null);
        }
        catch (Exception ex)
        {
            return (false, null, $"Error: {ex.Message}");
        }
    }

    public async Task<List<EventAttendance>> GetEventAttendances(Guid userId)
    {
        return await _context.EventAttendances
            .Where(ea => ea.Id == userId)
            .ToListAsync(); // Haal de attendances op voor een specifieke gebruiker
    }


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
}
