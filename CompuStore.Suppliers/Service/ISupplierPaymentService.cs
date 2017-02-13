using CompuStore.Suppliers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Suppliers.Service
{
    public interface ISupplierPaymentService
    {
        bool Add(SupplierPayment supplierPayment);
        bool Update(SupplierPayment supplierPayment);
        bool Delete(SupplierPayment supplierPayment);
        IEnumerable<SupplierPayment> GetAll(Supplier supplier);
        IEnumerable<SupplierPayment> SearchByInterval(Supplier supplier, DateTime from, DateTime to);
        SupplierPayment Find(int iD);
    }
}
