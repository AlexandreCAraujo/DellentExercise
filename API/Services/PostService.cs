using API.Models;
using System.Runtime.Caching;

namespace API.Services
{
    public class PostService : IPostService
    {

        /// <summary>
        /// This method uses an instance of the HttpClient class, that is responsible for asynchronously coordenating the Http resquest 
        /// to the third-party endpoint 
        /// </summary>
        /// <returns>The method returns an array of posts in the form of a Task, since it is asynchronous</returns>
        public async Task<Post[]> GetPostsFromEndpoint()
        {
            HttpClient client = new HttpClient();
            var baseUrl = "https://jsonplaceholder.typicode.com/";
            client.BaseAddress = new Uri(baseUrl);
            var endpoint = "/posts";

            HttpResponseMessage response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadFromJsonAsync<Post[]>();
                if (responseBody == null)
                {
                    throw new Exception("There was an error while fetching the Posts");
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
        /// <returns>An array of posts</returns>
        public Post[] GetPostsFromCache()
        {
            MemoryCache cache = MemoryCache.Default;
            var result = cache.Get("posts") as Post[];

            //If the method can't find nothing in cache it will run the GetPostsFromEndpoint() method and store it in cache
            if (result == null)
            {
                result = GetPostsFromEndpoint().Result;
                //The Sliding Expiration is the time that the cache policy will persist
                //In this case, after a minute this cache will expire, making it necessary to fetch new data in a following execution
                cache.Add("posts", result, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromSeconds(60) });
            }

            return result;
        }
    }
}
