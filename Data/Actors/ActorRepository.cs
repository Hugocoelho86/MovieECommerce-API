using ECommerceMovies.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMovies.API.Data.Actors
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        public ActorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public  IEnumerable<Actor> GetActorsWithMovies()
        {
            return _context.Actors.Include(a => a.Movies).ToList();
        }
    }
}
