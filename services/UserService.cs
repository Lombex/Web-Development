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
<<<<<<< HEAD
        _context = context;
=======
        return await Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
>>>>>>> PointsMethods
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
<<<<<<< HEAD
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> UpdateUserAsync(Guid id, User updatedUser) //update user
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

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
=======
        var GetUser = user.Id == Guid.Empty ? Guid.NewGuid() : user.Id;
        user.Id = GetUser;
        _users.Add(user);
        Console.WriteLine($"Added User to list: {user.Id}");
        return await Task.FromResult(user);
    }
    public async Task<User?> UpdateUserAsync(Guid id, User user)
    {
        var index = _users.FindIndex(u => u.Id == id);
        if (index == -1) return null;
        
        _users[index].Firstname = user.Firstname;
        _users[index].Email = user.Email;
        return await Task.FromResult(_users[index]);
>>>>>>> PointsMethods
    }

    async Task<bool> IUserService.DeleteUserAsync(Guid id)
    {
<<<<<<< HEAD
        var user =  await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
=======
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;
        
        _users.Remove(user);
        return await Task.FromResult(true);
>>>>>>> PointsMethods
    }
    public async Task<IEnumerable<User>> GetAllUsersAsync() //get all users
    {
        return await _context.Users.ToListAsync();
    }
}
