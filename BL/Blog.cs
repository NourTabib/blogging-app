namespace WebApplication1.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string? Url { get; set; }
        public string? Category { get; set; }
        public virtual List<Post>? Posts { get; set; }
    }
}
