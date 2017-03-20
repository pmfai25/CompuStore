using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Prism.Interactivity.InteractionRequest;

namespace CompuStore.Suppliers.Confirmations
{
    class SupplierPurchaseConfirmation:Confirmation
    {
        public  Purchase SupplierPurchase { get; set; }
        public SupplierPurchaseConfirmation()
        {
            SupplierPurchase = new Purchase();
        }
        public SupplierPurchaseConfirmation(Purchase supplierPurchase)
        {
            SupplierPurchase = supplierPurchase;
        }
    }
}
