using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepositor;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;
        // define Repository<ShoppingCart> so that _db will become ApplicationDbContext
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            // base(db) is for parent class Repository
            // so that _db in Repository will become ApplicationDbContext
            // and dbSet will become ShoppingCart table in database
            _db = db;
        }

        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }
    }
}
