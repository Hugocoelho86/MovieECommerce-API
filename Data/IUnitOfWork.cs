using ECommerceMovies.API.Data.Actors;
using ECommerceMovies.API.Data.Movies;

namespace ECommerceMovies.API.Data
{
    public interface IUnitOfWork
    {
        IMovieRepository Movie { get; }
        IActorRepository Actor { get; }

        Task<int> SaveAsync();
    }
}
