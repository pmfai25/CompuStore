﻿using System.Collections.Generic;
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
        bool IsSupplierWithPurchases(Supplier selectedItem);
    }
}
