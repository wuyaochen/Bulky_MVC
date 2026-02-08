using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepositor
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj); 
        // This method is specific to the Category repository.
        // it may have additional logic for updating a category.
    }
}
