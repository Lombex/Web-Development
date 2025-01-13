using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IAttendanceService
{
    Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterAttendance(Attendance attendance);
    Task<List<Attendance>> GetUserAttendances(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> RemoveAttendance(Guid id);
}

public class AttendanceService : IAttendanceService
{
    private readonly AppDbContext _context;

    public AttendanceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Attendance>> GetUserAttendances(Guid userId)
    {
        return await _context.Attendances
            .Where(a => a.UserId == userId)
            .Include(a => a.EventAttendance)
            .ThenInclude(ea => ea.Event)
            .ToListAsync();
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> RemoveAttendance(Guid id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null)
        {
            return (false, "Attendance niet gevonden.");
        }

        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();
        return (true, null);
    }
    public async Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterAttendance(Attendance attendance)
{
    try
    {
        // Controleer of de EventAttendance bestaat
        var eventAttendanceExists = await _context.EventAttendances
            .AnyAsync(ea => ea.Id == attendance.EventAttendanceId);

        if (!eventAttendanceExists)
        {
            return (false, null, "The associated EventAttendance does not exist.");
        }

        // Controleer op duplicaten
        var existingAttendance = await _context.Attendances
            .FirstOrDefaultAsync(a => a.UserId == attendance.UserId && a.EventAttendanceId == attendance.EventAttendanceId);

        if (existingAttendance != null)
        {
            return (false, null, "Attendance already exists for this user and event.");
        }

        // Voeg nieuwe Attendance toe
        await _context.Attendances.AddAsync(attendance);
        await _context.SaveChangesAsync();
        return (true, "Attendance succesvol geregistreerd.", null);
    }
    catch (Exception ex)
    {
        return (false, null, $"Er is een fout opgetreden: {ex.Message}");
    }
}

}
