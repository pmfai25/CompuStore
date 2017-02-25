﻿using Model;
using Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IPurchaseService
    {
        IEnumerable<SupplierPurchases> GetSupplierPurchases(Supplier supplier);
        IEnumerable<SupplierPurchases> GetSupplierPurchases(Supplier supplier, DateTime dateFrom, DateTime dateTo);
        IEnumerable<SupplierPurchases> GetSupplierPurchases(DateTime dateFrom, DateTime dateTo);
        IEnumerable<PurchaseDetails> GetPurchaseDetails(SupplierPurchases purchase);
        bool AddPurchase(Purchase purchase);
        bool UpdatePurchase(Purchase purchase);
        bool DeletePurchase(Purchase purchase);
        bool AddPurchaseItems(List<PurchaseItem> purchaseItem);
        bool UpdatePurchaseItems(List<PurchaseItem> purchaseItem);
        bool DeletePurchaseItems(List<PurchaseItem> purchaseItem);
        SupplierPurchases FindPurchaseDetails(int purchaseID);
    }
}