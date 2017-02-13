using System.Collections.Generic;
using CompuStore.Suppliers.Model;
namespace CompuStore.Suppliers.Service
{
    public interface ISupplierService
    {
        bool Add(Supplier supplier);
        bool Update(Supplier supplier);
        bool Delete(Supplier supplier);
        IEnumerable<Supplier> SearchBy(string name);
        IEnumerable<Supplier> GetAll();
        Supplier Find(int id);
        bool IsSupplierWithPurchases(Supplier selectedItem);
    }
}
