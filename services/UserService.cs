using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public interface IUserService
{
    Task<User?> GetUserAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(Guid id, User user);
    Task<bool> DeleteUserAsync(Guid id);
    Guid GetUserIdFromToken(string token);
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }


    public async Task<User> CreateUserAsync(User user)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(user.Email))
            throw new InvalidOperationException("Email is required");

        if (string.IsNullOrWhiteSpace(user.Password))
            throw new InvalidOperationException("Password is required");

        // Check for existing email
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already exists");

        // Set default values
        user.Id = Guid.NewGuid();
        user.Role = user.Role == 0 ? UserRole.User : user.Role;
        user.Points ??= new UserPointsModel
        { 
            AllTimePoints = 1000, 
            PointAmount = 0, 
            Items = new List<ShopItems>() 
        };

        // Save the new user
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // After user is created, initialize the streak
        var streak = new Streak
        {
            UserId = user.Id, // Set the UserId for the streak
            User = user, // Set the User reference for the streak
            CurrentStreak = 0, // Initialize the streak with 0
            HighestStreak = 0, // Set default highest streak value to 0
            LastAttendance = DateTime.Now // Set the default last attendance date
        };

        // Add the Streak object to the database
        _context.Streaks.Add(streak);
        await _context.SaveChangesAsync();

        return user;
    }






    public async Task<User?> UpdateUserAsync(Guid id, User updatedUser)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return null;
        }
        user.Firstname = updatedUser.Firstname ?? user.Firstname;
        user.Lastname = updatedUser.Lastname ?? user.Lastname;
        user.Email = updatedUser.Email ?? user.Email;
        user.Password = updatedUser.Password ?? user.Password;
        user.RecuringDays = updatedUser.RecuringDays > 0 ? updatedUser.RecuringDays : user.RecuringDays;

        await _context.SaveChangesAsync();
        return user;
    }


    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

        public Guid GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (securityToken == null)
            throw new SecurityTokenException("Invalid token");

        var userId = securityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        return Guid.Parse(userId);
    }
}
