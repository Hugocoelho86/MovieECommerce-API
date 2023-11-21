namespace ECommerceMovies.API.Models
{
    public class Actor: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; } = new List<Movie>();
    }
}
