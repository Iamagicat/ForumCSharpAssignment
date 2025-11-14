namespace ApiContracts.CommentDtos;

public class CreateCommentDto
{
    public string Body { get; set; } = null!;
    public int UserId { get; set; }
    public int PostId { get; set; }
}