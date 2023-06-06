using API.Models;
using API.Responses;
using System.Runtime.Caching;

namespace API.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        /// This method uses an instance of the HttpClient class, that is responsible for asynchronously coordenating the Http resquest 
        /// to the third-party endpoint 
        /// </summary>
        /// <returns>The method returns an array of users in the form of a Task, since it is asynchronous</returns>
        public async Task<User[]> GetUsersFromEndpoint()
        {

            HttpClient client = new HttpClient();
            var baseUrl = "https://jsonplaceholder.typicode.com/";
            client.BaseAddress = new Uri(baseUrl);
            var endpoint = "/users";

            HttpResponseMessage response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadFromJsonAsync<User[]>();
                if (responseBody == null)
                {
                    throw new Exception("There was an error while fetching the Users");
                }

                return responseBody;
            }
            else
            {
                throw new Exception("There was an error while while handling the Http request");
            }


        }

        /// <summary>
        /// This method is responsible for fetching and managing the data stored in memory cache.
        /// </summary>
        /// <returns>An array of users</returns>
        public User[] GetUsersFromCache()
        {
            MemoryCache cache = MemoryCache.Default;
            var result = cache.Get("users") as User[];

            //If the method can't find nothing in cache it will run the GetUsersFromEndpoint() method and store it in cache
            if (result == null)
            {
                result = GetUsersFromEndpoint().Result;
                //The Sliding Expiration is the time that the cache policy will persist
                //In this case, after a minute this cache will expire, making it necessary to fetch new data in a following execution
                cache.Add("users", result, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromSeconds(60) });
            }

            return result;
        }

        /// <summary>
        /// This method takes an array of Users and an array of Posts in their raw forms and adapts them into the requested response format
        /// </summary>
        /// <param name="users">An array of Users</param>
        /// <param name="posts">An array of Posts</param>
        /// <returns>Returning an array of UserResponces which is a class that implements the required format for presentation</returns>
        public UserResponse[] ToUserResponse(User[] users, Post[] posts)
        {
            var userResponse = new UserResponse[users.Length];


            for (var i = 0; i < users.Length; i++)
            {
                var userPosts = new List<PostResponse>();
                userResponse[i] = new UserResponse
                {
                    Id = users[i].Id,
                    Name = users[i].Name,
                    UserName = users[i].UserName,
                    Email = users[i].Email,
                    Address = users[i].Address.Street
                             + ", " + users[i].Address.Suite
                             + " - " + users[i].Address.Zipcode
                             + " " + users[i].Address.City,
                    Phone = users[i].Phone,
                    WebSite = users[i].WebSite,
                    Company = users[i].Company.Name
                };



                for (var j = 0; j < posts.Length; j++)
                {
                    if (userResponse[i].Id == posts[j].UserId)
                    {
                        userPosts.Add(
                            new PostResponse
                            {
                                Id = posts[j].Id,
                                Title = posts[j].Title,
                                Body = posts[j].Body
                            });
                    }
                }
                userResponse[i].Posts = userPosts.ToArray();
            }
            return userResponse;

        }
    }
}
