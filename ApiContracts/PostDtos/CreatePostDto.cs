namespace ApiContracts.PostDtos;

public class CreatePostDto
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public int UserId { get; set; }
}