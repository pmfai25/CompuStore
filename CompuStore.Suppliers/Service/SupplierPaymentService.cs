using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompuStore.Suppliers.Model;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using Dapper;

namespace CompuStore.Suppliers.Service
{
    public class SupplierPaymentService : ISupplierPaymentService
    {
        private SqlConnection Connection;
        public bool Add(SupplierPayment supplierPayment)
        {
            return Connection.Insert(supplierPayment) != 0;
        }

        public bool Delete(SupplierPayment supplierPayment)
        {
            return Connection.Delete(supplierPayment);
        }

        public SupplierPayment Find(int iD)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", iD);
            return Connection.QuerySingle<SupplierPayment>("Select * from SupplierPayment where ID=@ID", args);
        }

        public IEnumerable<SupplierPayment> SearchByInterval(Supplier supplier, DateTime from, DateTime to)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("From", from);
            args.Add("To", to);
            args.Add("SupplierID", supplier.ID);
            return Connection.Query<SupplierPayment>("Select * from SupplierPayment where SupplierID=@SupplierID and [Date] >= @From and [Date] <= @To", args);
        }

        public bool Update(SupplierPayment supplierPayment)
        {
            return Connection.Update(supplierPayment);
        }

        public IEnumerable<SupplierPayment> GetAll(Supplier supplier)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("SupplierID", supplier.ID);
            return Connection.Query<SupplierPayment>("Select * from SupplierPayment where SupplierID=@SupplierID", args);
        }

        public SupplierPaymentService(SqlConnection connection)
        {
            Connection = connection;
        }
    }
}
