using Model;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Suppliers.Confirmations
{
    public class SupplierConfirmation:Confirmation
    {
        public Supplier Supplier { get; set; }
        public SupplierConfirmation()
        {
            Title = "";
            Supplier = new Supplier();
        }
        public SupplierConfirmation(Supplier supplier)
        {
            Title = "";
            Supplier = supplier;
        }
    }
}
