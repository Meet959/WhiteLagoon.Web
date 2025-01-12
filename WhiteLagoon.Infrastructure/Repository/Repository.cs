using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> dbset;
        public Repository(ApplicationDbContext db) 
        {
            _db = db;
            dbset = _db.Set<T>();
        }
        public void Add(T Entity)
        {
            dbset.Add(Entity);
        }

        public T Get(Expression<Func<T, bool>> filter = null, string? IncludeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null) {
                query.Where(filter);
            }
            if (!string.IsNullOrEmpty(IncludeProperties))
            {
                foreach (var property in IncludeProperties.Split(new char[] { },StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T,bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(property);
                }
            }
            return query.ToList();
        }

        public void Remove(T Entity)
        {
            dbset.Add(Entity);
        }
    }
}
