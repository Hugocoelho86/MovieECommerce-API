using ECommerceMovies.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMovies.API.Data
{
    public class EcommerceMovieDbContext: DbContext
    {
        public EcommerceMovieDbContext(DbContextOptions<EcommerceMovieDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
    }
}
