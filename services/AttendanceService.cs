 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IAttendanceService
{
    Task<(bool IsSuccess, string Message, string ErrorMessage)> RegisterAttendance(Attendance attendance);
    Task<List<Attendance>> GetUserAttendances(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> RemoveAttendance(Guid id);
}

public class AttendanceService : IAttendanceService
{
    private readonly List<Attendance> _attendances = new(); 

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
        var userAttendances = _attendances.Where(a => a.UserId == userId).ToList();
        return await Task.FromResult<List<Attendance>>(userAttendances);
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> RemoveAttendance(Guid id)
    {
        var attendance = _attendances.FirstOrDefault(a => a.UserId == id);
        if (attendance == null)
        {
            return await Task.FromResult<(bool, string)>((false, "Attendance not found"));
        }

        _attendances.Remove(attendance);
        return await Task.FromResult<(bool, string)>((true, null));
    }
}
