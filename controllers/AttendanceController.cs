using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // POST: api/Attendance
        // Endpoint om een gebruiker zijn aanwezigheid te registreren
        [HttpPost]
        public async Task<IActionResult> RegisterAttendance([FromBody] Attendance attendance)
        {
            if (attendance == null || attendance.UserId == Guid.Empty || attendance.date == DateTime.MinValue)
            {
                return BadRequest("Invalid attendance data.");
            }

            var result = await _attendanceService.RegisterAttendance(attendance);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        // GET: api/Attendance/{userId}
        // Haal de lijst van aanwezigheden van een gebruiker op
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAttendances(Guid userId)
        {
            var attendances = await _attendanceService.GetUserAttendances(userId);
            if (attendances != null)
            {
                return Ok(attendances);
            }
            return NotFound("No attendances found for this user.");
        }

        // DELETE: api/Attendance/{id}
        // Verwijder een aanwezigheid van een gebruiker
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAttendance(Guid id)
        {
            var result = await _attendanceService.RemoveAttendance(id);
            if (result.IsSuccess)
            {
                return Ok("Attendance removed successfully.");
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}
