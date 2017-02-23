using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dapper.Contrib.Extensions;
using Dapper;
using System.Data;

namespace Service
{
    public class ItemService : IItemService
    {
        IDbConnection Connection;
        public bool Add(Item item)
        {
            return Connection.Insert(item) != 0;
        }

        public bool Delete(Item item)
        {
            return Connection.Delete(item);
        }

        public Item Find(int id)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", id);
            return Connection.QuerySingle<Item>("Select * from Item where ID=@ID", args);
        }

        public IEnumerable<Item> GetAll(bool simple=false)
        {
            if (simple)
                return Connection.Query<Item>("Select ID, Name, Serial from Item");
            return Connection.Query<Item>("Select * from Item");
        }
        
        public IEnumerable<Item> GetAll(int categoryID)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("CategoryID", categoryID);
            return Connection.Query<Item>("Select * from Item where CategoryID=@CategoryID", args);
        }

        public bool IsDeletable(Item item)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ItemID", item.ID);
            return Connection.QuerySingle<int>("Select count(*) from OrderItem oi, PurchaseItem pi where oi.ItemID=@ItemID or pi.ItemID=@ItemID", args) == 0;
        }

        public IEnumerable<Item> SearchBy(int categoryID, long serial)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("CategoryID", categoryID);
            args.Add("Serial", serial);
            return Connection.Query<Item>("Select * from Item where CategoryID=@CategoryID and Serial=@Serial", args);
        }
        public IEnumerable<Item> SearchBy(int categoryID, string name)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("CategoryID", categoryID);
            args.Add("Name", name+"%");
            return Connection.Query<Item>("Select * from Item where CategoryID=@CategoryID and  Name like @Name", args);
        }

        public bool Update(Item item)
        {
            return Connection.Update(item);
        }

        public Item SearchBySerial(long serial)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("Serial", serial);
            return Connection.QueryFirstOrDefault<Item>("Select * from Item where Serial=@Serial limit 1", args);
        }

        public ItemService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
