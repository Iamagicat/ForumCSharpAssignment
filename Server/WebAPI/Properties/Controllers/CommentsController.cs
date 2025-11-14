using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commRepo;

    public CommentsController(ICommentRepository commRepo)
    {
        this.commRepo = commRepo;
    }


    [HttpPost]

    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto commentDto)
    {

        var comment = new Comment()
        {
            Body = commentDto.Body,
            UserId = commentDto.UserId,
            PostId = commentDto.PostId
        };

        var created = await commRepo.AddAsync(comment);

        var result = new CommentDto()
        {
            Id = created.Id,
            Body = created.Body,
            UserId = created.UserId,
            PostId = created.PostId

        };
        return Created($"/Comments/{created.Id}", result);
    }

    [HttpGet]

    public async Task<IEnumerable<CommentDto>> GetMany([FromQuery] int? userId,  [FromQuery] int? postId)
    {
        var query = commRepo.GetMany();
        if (userId != null)
        {
            query = query.Where(c => c.UserId == userId);
        }
        
        if (postId != null)
        {
            query = query.Where(c => c.PostId == postId);
        }

        return query.Select(c => new CommentDto
        {
            Id = c.Id,
            Body = c.Body,
            UserId = c.UserId,
            PostId = c.PostId
        });
    }

    [HttpGet("{id:int}")]

    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        Comment comment;
        try
        {
            comment = await commRepo.GetSingleAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }

        return new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            UserId = comment.UserId,
            PostId = comment.PostId
        };

    }

    [HttpPut("{id:int}")]

    public async Task<IActionResult> Update(int id, [FromBody] CommentDto commentDto)
    {
        var updated = new Comment
        {
            Body = commentDto.Body,
            UserId = commentDto.UserId,
            PostId = commentDto.PostId,
            Id = id
        };

        try
        {
            await commRepo.UpdateAsync(updated);
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
            await commRepo.DeleteAsync(id);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        return NoContent();
    }

}



/*{
public int Id { get; set; }
public string? Body{ get; set; }
public int UserId { get; set; }
public int PostId { get; set; }
}*/ 