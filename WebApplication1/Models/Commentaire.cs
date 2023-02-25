namespace WebApplication1.Models
{
    public class Commentaire
    {
        public int Id { get; set; }
        public virtual Post? post { get; set; }
        public String? containt { get; set; }
        public DateTime? postedOn { get; set; }
        public virtual ApplicationUser? author { get; set; }
    }
}
