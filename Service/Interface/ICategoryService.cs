using Model;
using System.Collections.Generic;

namespace Service
{
    public interface ICategoryService
    {
        bool Add(Category category);
        bool Update(Category category);
        bool Delete(Category category);
        bool IsDeletable(Category category);
        IEnumerable<Category> GetAll();
        Category Find(int id);
    }
}
