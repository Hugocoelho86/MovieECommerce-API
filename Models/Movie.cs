namespace ECommerceMovies.API.Models
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public int ReleaseYear { get; set; }
        public ICollection<OrderItem> Items { get; } = new List<OrderItem>();
        public ICollection<Actor> Actors { get; } = new List<Actor>();
    }
}
