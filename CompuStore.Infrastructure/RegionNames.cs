using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Infrastructure
{
    public static class RegionNames
    {
        public const string MainContentRegion = "MainContentRegion";
        public const string PurchasesMain = "PurchasesMain";
        public const string SalesMain = "SalesMain";
        
        public const string ReportsMain = "ReportsMain";
        #region Store Module
        public const string StoreMain = "StoreMain";
        public const string StoreEdit = "StoreEdit";
        public const string CategoryEdit = "CategoryEdit";
        #endregion
        #region Client Module 
        public const string ClientsMain = "ClientsMain";
        public const string ClientEdit = "ClientEdit";
        public const string ClientPaymentMain = "ClientPaymentMain";
        public const string ClientPaymentEdit = "ClientPaymentEdit";
        public const string ClientSalesMain = "ClientSalesMain";
        #endregion
        #region Supplier Module
        public const string SuppliersMain = "SuppliersMain";
        public const string SupplierEdit = "SupplierEdit";
        public const string SupplierPaymentMain = "SupplierPaymentMain";
        public const string SupplierPaymentEdit = "SupplierPaymentEdit";
        public const string SupplierPurchasesMain = "SupplierPurchasesMain";
        #endregion
    }
}
