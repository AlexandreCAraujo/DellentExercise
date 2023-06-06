using API.Models;

namespace API.Services
{
    public interface IPostService
    {
        Task<Post[]> GetPostsFromEndpoint();
        Post[] GetPostsFromCache();
    }
}
