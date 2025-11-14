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
            Id = created.Id,
            Body = created.Body,
            Title = created.Title,
            UserId = created.UserId
        };

        return Created($"/Posts/{result.Id}", result);

    }

    [HttpGet]

    public async Task<IEnumerable<PostDto>> GetMany([FromQuery] string? title, [FromQuery] int? userId)
    {
        var query = postRepository.GetMany();

        if (title != null)
        {
            query = query.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
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

    [HttpGet("{id:int}")]

    public async Task<ActionResult<PostDto>> GetSIngle(int id)
    {
        Post post;
        try
        {
            post = await postRepository.GetSingleAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }

        return new PostDto
        {
            Body = post.Body,
            Id = post.Id,
            Title = post.Title,
            UserId = post.UserId
        };
    }

    [HttpPut("{id:int}")]

    public async Task<IActionResult> Update(int id, [FromBody] CreatePostDto postDto)
    {
        var updated = new Post
        {
            Id = id,
            Body = postDto.Body,
            Title = postDto.Title,
            UserId = postDto.UserId
        };

        try
        {
            await postRepository.UpdateAsync(updated);
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
            await postRepository.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }

        return NoContent();
    }








}