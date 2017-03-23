using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Prism.Interactivity.InteractionRequest;
namespace CompuStore.Suppliers.Confirmations
{
    public class SupplierPaymentConfirmation:Confirmation
    {
        public SupplierPayment SupplierPayment { get; set; }
        public SupplierPaymentConfirmation(int supplierID)
        {
            Title = "";
            SupplierPayment = new SupplierPayment();
            SupplierPayment.SupplierID = supplierID;
        }
        public SupplierPaymentConfirmation(SupplierPayment supplierPayment)
        {
            Title = "";
            SupplierPayment = supplierPayment;
        }
    }
}
