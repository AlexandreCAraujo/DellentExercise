using API.Models;
using API.Responses;

namespace API.Services
{
    public interface IUserService
    {
        Task<User[]> GetUsersFromEndpoint();
        User[] GetUsersFromCache();
        UserResponse[] ToUserResponse(User[] users, Post[] posts);
    }
}
