using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Prism.Interactivity.InteractionRequest;
namespace CompuStore.Suppliers.Confirmations
{
    class SupplierPaymentConfirmation:Confirmation
    {
        public SupplierPayment SupplierPayment { get; set; }
        public SupplierPaymentConfirmation()
        {
            SupplierPayment = new SupplierPayment();
        }
        public SupplierPaymentConfirmation(SupplierPayment supplierPayment)
        {
            SupplierPayment = supplierPayment;
        }
    }
}
