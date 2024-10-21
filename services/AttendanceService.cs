<<<<<<< HEAD
 using System;
=======
using System;
>>>>>>> PointsMethods
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IAttendanceService
{
    Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterAttendance(Attendance attendance);
<<<<<<< HEAD
    Task<List<Attendance>> GetUserAttendances(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> RemoveAttendance(Guid id);
=======
    Task<List<Attendance>> GetUserAttendances(Guid userId); // Gebruik int voor identificatie
    Task<(bool IsSuccess, string ErrorMessage)> RemoveAttendance(Guid id); // Gebruik Guid voor identificatie van attendance
>>>>>>> PointsMethods
}

public class AttendanceService : IAttendanceService
{
<<<<<<< HEAD
    private readonly List<Attendance> _attendances = new(); 
=======
    private readonly List<Attendance> _attendances = new();  // In-memory lijst voor Attendance
>>>>>>> PointsMethods

    public async Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterAttendance(Attendance attendance)
    {
        try
        {
            _attendances.Add(attendance);
            return await Task.FromResult<(bool, string, string)>((true, "Attendance registered successfully", null));
        }
        catch (Exception ex)
        {
            return await Task.FromResult<(bool, string, string)>((false, null, $"Error: {ex.Message}"));
        }
    }

    public async Task<List<Attendance>> GetUserAttendances(Guid userId)
    {
<<<<<<< HEAD
        var userAttendances = _attendances.Where(a => a.UserId == userId).ToList();
=======
        var userAttendances = _attendances.Where(a => a.UserID == userId).ToList();
>>>>>>> PointsMethods
        return await Task.FromResult<List<Attendance>>(userAttendances);
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> RemoveAttendance(Guid id)
    {
<<<<<<< HEAD
        var attendance = _attendances.FirstOrDefault(a => a.UserId == id);
=======
        var attendance = _attendances.FirstOrDefault(a => a.UserID == id);
>>>>>>> PointsMethods
        if (attendance == null)
        {
            return await Task.FromResult<(bool, string)>((false, "Attendance not found"));
        }

        _attendances.Remove(attendance);
        return await Task.FromResult<(bool, string)>((true, null));
    }
}
