// UserService.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    Task<User> GetUserAsync(Guid id);
    Task<User> UpdateUserAsync(Guid id, User updatedUser);
    Task<bool> DeleteUserAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync(); // method to get all users
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user) //create user
    {
        var newUser = new User(
            Guid.NewGuid(),
            user.Firstname,
            user.Lastname,
            user.Email,
            user.Password,
            user.RecuringDays,
            user.Role
        );

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<User> GetUserAsync(Guid id) // get user by id
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> UpdateUserAsync(Guid id, User updatedUser) //update user
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // Create a new user with updated fields
        var updatedUserEntity = new User(
            user.Id,
            updatedUser.Firstname,
            updatedUser.Lastname,
            updatedUser.Email,
            updatedUser.Password,
            updatedUser.RecuringDays,
            updatedUser.Role
        );


        _context.Users.Remove(user);
        _context.Users.Add(updatedUserEntity);

        await _context.SaveChangesAsync();

        return updatedUserEntity;
    }

    async Task<bool> IUserService.DeleteUserAsync(Guid id)
    {
        var user =  await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<User>> GetAllUsersAsync() //get all users
    {
        return await _context.Users.ToListAsync();
    }
}
