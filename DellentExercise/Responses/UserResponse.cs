
namespace API.Responses
{
    //This class represents the required response format for Users
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? WebSite { get; set; }
        public string? Company { get; set; }
        public PostResponse[]? Posts { get; set; }


    }
}
