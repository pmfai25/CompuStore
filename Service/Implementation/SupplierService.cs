using System.Collections.Generic;
using Model;
using Dapper;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using System.Data;
using System;

namespace Service
{
    public class SupplierService : ISupplierService
    {
        IDbConnection Connection;
        public SupplierService(IDbConnection con)
        {
            Connection = con;
        }
        public bool Add(Supplier supplier)
        {
            try
            { return Connection.Insert(supplier) != 0; }
            catch { return false; }
        }        
        public bool Update(Supplier supplier)
        {
            try
            {
                return Connection.Update(supplier);
            }
            catch { return false; }
        }
        public bool Delete(Supplier supplier)
        {
            return Connection.Delete(supplier);
        }
        public IEnumerable<Supplier> GetAll(bool simple=false)
        {
            if (simple)
                return Connection.Query<Supplier>("Select ID, Name, Phone from Supplier");
            else
                return Connection.GetAll<Supplier>();
        }
        public IEnumerable<Supplier> SearchBy(string name)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("Name", name+"%");
            return Connection.Query<Supplier>("Select * from Supplier where Name like @Name or Phone like @Name", args);

        }
        public Supplier Find(int id)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", id);
            return Connection.QuerySingle<Supplier>("Select * from Supplier where ID=@ID", args);
        }

        public bool IsDeleteable(Supplier selectedItem)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", selectedItem.ID);
            return Connection.QuerySingle<decimal>("Select Purchases from Supplier where ID=@ID", args) == 0;
        }

        public List<Purchase> GetPurchases(Supplier supplier,DateTime dateFrom, DateTime dateTo)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("SupplierID", supplier.ID);
            args.Add("DateFrom", dateFrom);
            args.Add("DateTo", dateTo);
            return new List<Purchase>(Connection.Query<Purchase>("Select * from Purchase where SupplierID=@SupplierID and Date <=@DateTo and Date>=@DateFrom", args));
        }

        public List<Purchase> GetPurchases(Supplier supplier)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("SupplierID", supplier.ID);
            return new List<Purchase>(Connection.Query<Purchase>("Select * from Purchase where SupplierID=@SupplierID", args));
        }
    }
}
