using CompuStore.Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Store.Service
{
    public interface IItemService
    {
        bool Add(Item item);
        bool Update(Item item);
        bool Delete(Item item);
        bool IsItemDeletable(Item item);
        IEnumerable<Item> SearchBy(string name, long serial);
        IEnumerable<Item> GetAll();
        IEnumerable<Item> GetAll(int categoryID);
        Item Find(int id);
    }
}
