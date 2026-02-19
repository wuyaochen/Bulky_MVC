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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;
        // define Repository<Category> so that _db will become ApplicationDbContext
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            // base(db) is for parent class Repository
            // so that _db in Repository will become ApplicationDbContext
            // and dbSet will become Category table in database
            _db = db;
        }

        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
