using ECommerceMovies.API.Models;

namespace ECommerceMovies.API.Data.Actors
{
    public interface IActorRepository: IRepository<Actor>
    {
        IEnumerable<Actor> GetActorsWithMovies();
    }
}
