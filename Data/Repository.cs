using ECommerceMovies.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace ECommerceMovies.API.Data
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity : BaseEntity
    {
        internal ApplicationDbContext _context;
        internal DbSet<TEntity> _entities;

        public Repository(ApplicationDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._entities = context.Set<TEntity>();
        }


        public virtual IEnumerable<TEntity> GetAllAsync()
        {
            return _entities.ToList();
        }

        public virtual TEntity GetByIdAsync(object id)
        {
            return _entities.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            _context.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _entities.Find(id);
            _entities.Remove(entityToDelete);
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }


        public virtual void Update(TEntity entityToUpdate)
        {
            _entities.Attach(entityToUpdate);
            _entities.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
