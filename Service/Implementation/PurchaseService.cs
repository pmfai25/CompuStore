﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Views;
using System.Data;
using Dapper.Contrib.Extensions;
using Dapper;

namespace Service
{
    class PurchaseService : IPurchaseService
    {
        IDbConnection Connection;
        public IEnumerable<SupplierPurchases> GetSupplierPurchases(Supplier supplier)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("SupplierID", supplier.ID);
            return Connection.Query<SupplierPurchases>("Select * from SupplierPurchases where SupplierID=@SupplierID", args);
        }
        public IEnumerable<SupplierPurchases> GetSupplierPurchases(Supplier supplier, DateTime dateFrom, DateTime dateTo)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("SupplierID", supplier.ID);
            args.Add("DateFrom", dateFrom);
            args.Add("DateTo", dateTo);
            return Connection.Query<SupplierPurchases>("Select * from SupplierPurchases where Date<=@DateTo and Date >=@DateFrom and SupplierID=@SupplierID", args);
        }
        public IEnumerable<SupplierPurchases> GetSupplierPurchases(DateTime dateFrom, DateTime dateTo)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("DateFrom", dateFrom);
            args.Add("DateTo", dateTo);
            Dictionary<int, SupplierPurchases> dict = new Dictionary<int, SupplierPurchases>();
            Connection.Query<SupplierPurchases, PurchaseDetails, SupplierPurchases>("Select p.*,d.* from SupplierPurchases p inner join PurchaseDetails d on p.PurchaseID=d.PurchaseID where  Date<=@DateTo and Date >=@DateFrom",
                   (c, d) =>
                   {
                       if (!dict.ContainsKey(c.PurchaseID))
                           dict.Add(c.PurchaseID, c);
                       dict[c.PurchaseID].Details.Add(d);
                       return c;
                   }, args, splitOn: "PurchaseID");
            return dict.Values;

        }
        public IEnumerable<PurchaseDetails> GetPurchaseDetails(int purchaseID)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("PurchaseID", purchaseID);
            return Connection.Query<PurchaseDetails>("Select * from PurchaseDetails where PurchaseID=@PurchaseID", args);
        }

        public bool AddPurchase(Purchase purchase)
        {
            return Connection.Insert(purchase) != 0;
        }

        public bool AddPurchaseItems(List<PurchaseItem> purchaseItem)
        {
            return Connection.Insert(purchaseItem) != 0;
        }

        public bool DeletePurchase(Purchase purchase)
        {
            return Connection.Delete(purchase);
        }

        public bool DeletePurchaseItems(List<PurchaseItem> purchaseItem)
        {
            return Connection.Delete(purchaseItem);
        }
        public bool UpdatePurchase(Purchase purchase)
        {
            return Connection.Update(purchase);
        }

        public bool UpdatePurchaseItems(List<PurchaseItem> purchaseItem)
        {
            return Connection.Update(purchaseItem);
        }

        public Purchase FindPurchase(int purchaseID)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", purchaseID);
            return Connection.QuerySingle<Purchase>("Select * from Purchase where ID=@ID", args);
        }

        public bool IsDeletable(Purchase purchase)
        {
            Dictionary<Tuple<int, decimal>, int> dict = new Dictionary<Tuple<int, decimal>, int>();
            var lst = GetPatches(purchase);
            foreach (var p in lst)
            {
                var t = new Tuple<int, decimal>(p.ItemID, p.Price);
                if (!dict.ContainsKey(t))
                    dict.Add(t, p.CurrentQuantity);
                dict[t] -= p.Quantity;
            }
            return dict.All(x => x.Value >= 0);
        }
        public IEnumerable<PatchPurchases> GetPatches(Purchase purchase)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("PurchaseID", purchase.ID);
            return Connection.Query<PatchPurchases>("Select * from PatchPurchases where PurchaseID=@PurchaseID",args);
        }

        public int GetAvailableQuantity(int itemID, decimal price)
        {
            throw new NotImplementedException();
        }

        public PurchaseService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
