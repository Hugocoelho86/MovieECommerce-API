using ECommerceMovies.API.Data.Actors;
using ECommerceMovies.API.Data.Movies;
using ECommerceMovies.API.Models;
using System.Transactions;

namespace ECommerceMovies.API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IMovieRepository Movie { get; private set; }
        public IActorRepository Actor { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            
            Movie = new MovieRepository(_context);
            Actor = new ActorRepository(_context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
