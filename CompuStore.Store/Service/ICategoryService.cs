using CompuStore.Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Store.Service
{
    public interface ICategoryService
    {
        bool Add(Category category);
        bool Update(Category category);
        bool Delete(Category category);
        bool IsDeletable(Category category);
        IEnumerable<Category> GetAll();
    }
}
