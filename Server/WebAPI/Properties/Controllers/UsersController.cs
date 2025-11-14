using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
    {
        var user = new User
        {
            UserName = dto.UserName,
            Password = dto.Password
        };

        var created = await userRepo.AddAsync(user);

        var result = new UserDto
        {
            Id = created.Id,
            UserName = user.UserName,
        };
        
        return Created($"/Users/{result.Id}", result);
    }

    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetMany([FromQuery] string? userName)
    {
        var query = userRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            query = query.Where(u => u.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase));
        }

        return query.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        User user;
        try
        {
            user = await userRepo.GetSingleAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUserDto dto)
    {
        var updated = new User
        {
            Id = id,
            UserName = dto.UserName,
            Password = dto.Password
        };

        try
        {
            await userRepo.UpdateAsync(updated);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }

        return NoContent();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
