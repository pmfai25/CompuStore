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
    public class SupplierService : ISuppliersService
    {
        SqlConnection Connection;
        public SupplierService(SqlConnection con)
        {
            Connection = con;
        }
        public bool Add(Supplier supplier)
        {
            Connection.Open();
            long x=Connection.Insert(supplier);
            Connection.Close();
            return x != 0;
        }

        public bool Delete(Supplier supplier)
        {
            Connection.Open();
            bool x = Connection.Delete(supplier);
            Connection.Close();
            return x;
        }

        public bool Update(Supplier supplier)
        {
            Connection.Open();
            bool x = Connection.Update(supplier);
            Connection.Close();
            return x;
        }

        public IEnumerable<SuppliersDetails> GetAll()
        {
            Connection.Open();
            IEnumerable<SuppliersDetails> x = Connection.GetAll<SuppliersDetails>();
            Connection.Close();
            return x;
        }

        public SuppliersDetails SearchByName(string name)
        {
            Connection.Open();
            SuppliersDetails x = Connection.QuerySingle<SuppliersDetails>("Select * from SuppliersDetails where Name like \'@Name%\'",name);
            Connection.Close();
            return x;
        }
    }
}
