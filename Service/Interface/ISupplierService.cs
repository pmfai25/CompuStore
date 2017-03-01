using System;
using System.Collections.Generic;
using Model;
namespace Service
{
    public interface ISupplierService
    {
        bool Add(Supplier supplier);
        bool Update(Supplier supplier);
        bool Delete(Supplier supplier);
        IEnumerable<Supplier> SearchBy(string name);
        IEnumerable<Supplier> GetAll(bool simple=false);
        Supplier Find(int id);
        bool IsDeleteable(Supplier selectedItem);
        List<Purchase> GetPurchases(Supplier supplier,DateTime dateFrom, DateTime dateTo);
        List<Purchase> GetPurchases(Supplier supplier);
    }
}
