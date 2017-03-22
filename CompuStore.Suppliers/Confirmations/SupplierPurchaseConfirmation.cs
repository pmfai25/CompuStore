using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;
using Model.Views;

namespace CompuStore.Suppliers.Confirmations
{
    public class SupplierPurchaseConfirmation:Confirmation
    {
        public  Purchase SupplierPurchase { get; set; }
        public ObservableCollection<PurchaseDetails> Details { get; set; }
        public SupplierPurchaseConfirmation()
        {
            Title = "";
            SupplierPurchase = new Purchase();
            Details = new ObservableCollection<PurchaseDetails>();
        }
        public SupplierPurchaseConfirmation(Purchase purchase)
        {
            Title = "";
            SupplierPurchase = purchase;
            Details = new ObservableCollection<PurchaseDetails>();
        }
        public SupplierPurchaseConfirmation(Purchase supplierPurchase, IEnumerable<PurchaseDetails> purchaseDetails)
        {
            Title = "";
            SupplierPurchase = supplierPurchase;
            Details = new ObservableCollection<PurchaseDetails>(purchaseDetails);
        }
    }
}
