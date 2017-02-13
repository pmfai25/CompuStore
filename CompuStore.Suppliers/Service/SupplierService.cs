using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompuStore.Suppliers.Model;
using CompuStore.Infrastructure;
using System.Data;
using Dapper.Contrib;
using Dapper;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;

namespace CompuStore.Suppliers.Service
{
    public class SupplierService : ISupplierService
    {
        SqlConnection Connection;
        public SupplierService(SqlConnection con)
        {
            Connection = con;
        }
        public bool Add(Supplier supplier)
        {
            return Connection.Insert(supplier) != 0;            
        }
        public bool Delete(Supplier supplier)
        {
            return Connection.Delete(supplier);
        }
        public bool Update(Supplier supplier)
        {
            return Connection.Update(supplier);
        }
        public IEnumerable<Supplier> GetAll()
        {
            return Connection.GetAll<Supplier>();
        }
        public IEnumerable<Supplier> SearchBy(string name)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("Name", name);
            return Connection.Query<Supplier>("Select * from Supplier where Name like @Name +'%' or Phone like @Name+ '%'", args);

        }
        public Supplier Find(int id)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", id);
            return Connection.QuerySingle<Supplier>("Select * from Supplier where ID=@ID", args);
        }

        public bool IsSupplierWithPurchases(Supplier selectedItem)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", selectedItem.ID);
            return Connection.QuerySingle<decimal>("Select Purchases from Suppliers where ID=@ID", args) != 0;
        }
    }
}
