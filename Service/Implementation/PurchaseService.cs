using System;
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
        public IEnumerable<PurchaseDetails> GetPurchaseDetails(int purchaseID)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("PurchaseID", purchaseID);
            var x= Connection.Query<PurchaseDetails>("Select * from PurchaseDetails where PurchaseID=@PurchaseID", args);
            foreach (var i in x)
                i.Sold = i.Quantity - i.Available;
            return x;
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
            DynamicParameters args = new DynamicParameters();
            args.Add("PurchaseID", purchase.ID);
            return Connection.Query<PurchaseItem>("Select * from PurchaseItem where PurchaseID=@PurchaseID", args).All(x => x.Available == x.Quantity);
        }
        public PurchaseService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
