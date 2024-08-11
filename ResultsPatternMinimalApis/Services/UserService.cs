using FluentResults;
using Microsoft.EntityFrameworkCore;
using ResultsPatternMinimalApis.Database;
using ResultsPatternMinimalApis.FluentModels;
using ResultsPatternMinimalApis.Models;

namespace ResultsPatternMinimalApis.Services;

public interface IUserService
{
    Task<Result<User>> GetUserByIdAsync(int id);

    Task<Result<User>> CreateUserAsync(User user);
}

public class UserService: IUserService
{
    private readonly ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<User>> GetUserByIdAsync(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        if (user is null)
        {
            return Result.Fail("User not found.")
                .WithError(new RecordNotFound("User not found."));
        }
        
        return Result.Ok(user);
    }
    
    public async Task<Result<User>> CreateUserAsync(User user)
    {
        var userAlreadyExists = await _dbContext.Users.AnyAsync(u => u.Id == user.Id);
        if (userAlreadyExists)
        {
            return Result.Fail("User already exists.")
                .WithError(new RecordAlreadyExists("User already exists."));    
        }
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return Result.Ok()
            .WithSuccess(new RecordCreated("User created succesfully."));
    }
}