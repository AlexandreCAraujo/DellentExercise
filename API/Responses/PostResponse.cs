namespace API.Responses
{
    //This class is the same as Post, but without an UserId
    public class PostResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
