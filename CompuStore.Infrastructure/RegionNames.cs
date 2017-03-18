using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Infrastructure
{
    public static class RegionNames
    {
        public const string SuppliersRegion = "SuppliersRegion";
        public const string ClientsRegion = "ClientsRegion";
        public const string ItemsRegion = "ItemsRegion";
        public const string ReportsRegion = "ReportsRegion";
        public const string RegisterNow = "RegisterNow";

        public const string MainContentRegion = "MainContentRegion";
        public const string NavContentRegion = "NavContentRegion";
        public const string SupplierPurchasesReport = "SupplierPurchasesReport";        
        public const string ClientOrdersReport = "ClientOrdersReport";
        public const string ReportIncomeOutcomeView = "ReportIncomeOutcomeView";
        public const string ReportsMain = "ReportsMain";
        public const string Settings = "Settings";
        public const string Login = "Login";
        #region Store Module
        public const string StoreMain = "StoreMain";
        public const string StoreEdit = "StoreEdit";
        public const string CategoryMain = "CategoryMain";
        #endregion
        #region Client Module 
        public const string ClientsMain = "ClientsMain";
        public const string ClientEdit = "ClientEdit";
        public const string ClientPaymentMain = "ClientPaymentMain";
        public const string ClientPaymentEdit = "ClientPaymentEdit";
        public const string ClientSalesMain = "ClientSalesMain";
        public const string ClientSaleEdit = "ClientSaleEdit";
        #endregion
        #region Supplier Module
        public const string SuppliersMain = "SuppliersMain";
        public const string SupplierEdit = "SupplierEdit";
        public const string SupplierPaymentMain = "SupplierPaymentMain";
        public const string SupplierPaymentEdit = "SupplierPaymentEdit";
        public const string SupplierPurchasesMain = "SupplierPurchasesMain";
        public const string SupplierPurchaseEdit = "SupplierPurchaseEdit";
        #endregion
    }
}
