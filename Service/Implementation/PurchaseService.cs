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
            DynamicParameters args = new DynamicParameters();
            args.Add("PurchaseID", purchase.ID);
            return Connection.Query<PurchaseItem>("Select * from PurchaseItem where PurchaseID=@PurchaseID", args).All(x => x.Available == x.Quantity);
        }

        public List<PurchaseItem> GetPurchaseItemsWithStock(Item item)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ItemID", item.ID);
            return new List<PurchaseItem>(Connection.Query<PurchaseItem>("Select * from PurchaseItem where Available>0 and ItemID=@ItemID", args));
        }

        public IEnumerable<SupplierPurchases> GetPurchases(DateTime from, DateTime to)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("DateFrom", from);
            args.Add("DateTo", to);
            return Connection.Query<SupplierPurchases>("Select * from SupplierPurchases where Date<=@DateTo and Date>=@DateFrom", args);
        }

        public IEnumerable<SupplierPurchases> GetPurchases()
        {
            return Connection.Query<SupplierPurchases>("Select * from SupplierPurchases");
        }

        public PurchaseService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
