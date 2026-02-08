using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepositor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet; 
        // define T, so dbSet will become Category table in database
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>(); 
            // _db.Categories == dbSet
        }
        public void Add(T entity)
        {
            _db.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
            // equal to Category.Controller (Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);)
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            _db.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _db.RemoveRange(entity);
        }
    }
}
