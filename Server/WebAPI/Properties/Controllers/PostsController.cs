using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;

    public PostsController(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    [HttpPost]

    public async Task<ActionResult<PostDto>> Create([FromBody] CreatePostDto postDto)
    {
        var post = new Post
        {
            Body = postDto.Body,
            Title = postDto.Title,
            UserId = postDto.UserId
        };
        
        var created = await postRepository.AddAsync(post);

        var result = new PostDto
        {
            Body = post.Body,
            Title = post.Title,
            UserId = created.UserId
        };
        
        return Created($"/Posts/{result.Id}", result);

    }

    [HttpGet]

    public async ActionResult<IEnumerable<PostDto>> GetMany([FromQuery] string? title, [FromQuery] int? userId)
    {
        var query = postRepository.GetMany();

        if (title != null)
        {
            query = query.Where(p => p.Title == title);
        }

        if (userId != null)
        {
            query = query.Where(p => p.UserId == userId);
        }

        return query.Select(p => new PostDto
            {
                Body = p.Body,
                Title = p.Title,
                UserId = p.UserId,
                Id = p.Id
            }
        );
    }
}

    
    
    
    
    
    
    
}