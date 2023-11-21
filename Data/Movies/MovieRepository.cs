using ECommerceMovies.API.Models;

namespace ECommerceMovies.API.Data.Movies
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
