 using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

public interface IAttendanceService
{
    Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterAttendance(Attendance attendance);
    Task<List<Attendance>> GetUserAttendances(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> RemoveAttendance(Guid id);
}

public class Attendance_service : IAttendanceService
{
    private readonly AppDbContext _context;

    public Attendance_service(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterAttendance(Attendance attendance)
    {
        try
        {
            await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync();
            return (true, "Attendance succesvol geregistreerd.", null);
        }
        catch (Exception ex)
        {
            // Fout loggen als je een logging framework hebt.
            return (false, null, $"Er is een fout opgetreden: {ex.Message}");
        }
    }

    public async Task<List<Attendance>> GetUserAttendances(Guid userId)
    {
        return await _context.Attendances.Where(a => a.UserId == userId).ToListAsync();
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
}
