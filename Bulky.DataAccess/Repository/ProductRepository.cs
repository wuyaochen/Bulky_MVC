using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepositor;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
            private ApplicationDbContext _db;
            // define Repository<Product> so that _db will become ApplicationDbContext
            public ProductRepository(ApplicationDbContext db) : base(db)
            {
                // base(db) is for parent class Repository
                // so that _db in Repository will become ApplicationDbContext
                // and dbSet will become Product table in database
                _db = db;
            }
    
            public void Update(Product obj)
            {
                _db.Products.Update(obj);
        }
    }
}
