using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        //Dependency injection
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        public UserController(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
        }


        /// <summary>
        /// This is the endpoint responsible for calling the methods that fetch data from the third-party endpoints and 
        /// the one that converts that data in the desired response model
        /// </summary>
        /// <returns>The result of the action</returns>
        [HttpGet("fetch")]
        [EnableQuery]
        //The EnableQuery attribute allows for OData operations on the response data
        public ActionResult Fetch()
        {
            var users = _userService.GetUsersFromCache();
            var posts = _postService.GetPostsFromCache();

            var result = _userService.ToUserResponse(users, posts);

            if (result == null)
            {
                return Problem("There was an error while building the response");
            }

            return Ok(result);

        }


    }
}
