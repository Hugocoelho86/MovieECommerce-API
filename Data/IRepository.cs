using ECommerceMovies.API.Models;
using System.Linq.Expressions;

namespace ECommerceMovies.API.Data
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAllAsync();
        TEntity GetByIdAsync(object id);
        void Add(TEntity entity);
        void Update(TEntity entityToUpdate);
        void Delete(object id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
    }
}
