using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    //The raw format of Post, as retrieved from the endpoint 
    public class Post
    {

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public int UserId { get; set; }
    }
}
