using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompuStore.Store.Model;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using Dapper;

namespace CompuStore.Store.Service
{
    class ItemService : IItemService
    {
        SqlConnection Connection;
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

        public IEnumerable<Item> GetAll()
        {
            return Connection.Query<Item>("Select * from Item");
        }

        public IEnumerable<Item> GetAll(int categoryID)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("CategoryID", categoryID);
            return Connection.Query<Item>("Select * from Item where CategoryID=@CategoryID", args);
        }

        public bool IsItemDeletable(Item item)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ItemID", item.ID);
            return Connection.QuerySingle<int>("Select count(*) from OrderItem oi, PurchaseItem pi where oi.ItemID=@ItemID or pi.ItemID=@ItemID", args) == 0;
        }

        public IEnumerable<Item> SearchBy(string name,long serial)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("Name", name);
            args.Add("Serial", serial);
            return Connection.Query<Item>("Select * from Item where Serial=@Serial or Name likes @Name+'%'", args);
        }

        public bool Update(Item item)
        {
            return Connection.Update(item);
        }
        public ItemService(SqlConnection connection)
        {
            Connection = connection;
        }
    }
}
