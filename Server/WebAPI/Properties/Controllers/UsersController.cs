using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Properties.Controllers;

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
    public async Task<ActionResult<UserDto>> AddUser([FromBody]) CreateUserDto request)
    {
        await VerifyUserNameIsAvailableAsync(request.UserName);

        User user = new(request.UserName, request.Paswword);
        User created = await userRepo.AddAsync(user);
        UserDto dto = new ()
        {
            id = created.id,
            UserName = created.UserName
        }
        return Created($"/users{dto.Id}", created);

    }
}
