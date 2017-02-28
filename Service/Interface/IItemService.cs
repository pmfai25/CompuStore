using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IItemService
    {
        bool Add(Item item);
        bool Update(Item item);
        bool Delete(Item item);
        bool IsDeletable(Item item);
        IEnumerable<Item> SearchBy(long serial);
        IEnumerable<Item> SearchBy( string name);
        Item SearchBySerial(long serial);       
        IEnumerable<Item> GetAll(bool simple=false);
        IEnumerable<Item> GetAll(int categoryID);
        Item Find(int id);
    }
}
