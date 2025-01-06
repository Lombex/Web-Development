using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    Task<User?> GetUserAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(Guid id, User user);
    Task<bool> DeleteUserAsync(Guid id);
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user) // Create user
    {
        user.Id = user.Id == Guid.Empty ? Guid.NewGuid() : user.Id; // Assign a new ID if it's empty
        await _context.Users.AddAsync(user); // Use AddAsync for asynchronous operation
        await _context.SaveChangesAsync(); // Save changes

        return user; // Return the created user
    }

    public async Task<User?> GetUserAsync(Guid id) // Get user by id
    {
        return await _context.Users.FindAsync(id); // Use FindAsync to find the user by ID
    }

    public async Task<User?> UpdateUserAsync(Guid id, User updatedUser) // Update user
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return null; // Return null if user not found
        }

        // Update user properties
        user.Firstname = updatedUser.Firstname;
        user.Lastname = updatedUser.Lastname;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password; // Consider hashing the password
        user.RecuringDays = updatedUser.RecuringDays;
        user.Role = updatedUser.Role;

        await _context.SaveChangesAsync(); // Save changes

        return user; // Return the updated user
    }

    public async Task<bool> DeleteUserAsync(Guid id) // Delete user
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false; // Return false if user not found
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(); // Save changes
        return true; // Return true if deleted
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync() // Get all users
    {
        return await _context.Users.ToListAsync(); // Return all users
    }
}
