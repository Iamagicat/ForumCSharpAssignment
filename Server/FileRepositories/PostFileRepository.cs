using System.Text.Json;
using Entities;
using RepositoryContracts;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "comments.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))

        {
            File.WriteAllText(filePath, "[]");

        }

    }

    public async Task<Post> AddAsync(Post post)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson)!;
        int maxId = posts.Count > 0 ? posts.Max(c => c.Id) : 1;
        post.Id = maxId + 1;
        posts.Add(post);
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postAsJson);
        return post;
    }

    public Task UpdateAsync(Post post)
    {
        throw new NotImplementedException();
    }
    

    public async Task DeleteAsync(int id)
    {
        
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        
    }

    public IQueryable<Post> GetMany()
    {
        throw new NotImplementedException();
    }
}