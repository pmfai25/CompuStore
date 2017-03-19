using Model;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Suppliers.Confirmations
{
    public class SupplierEditConfirmation:Confirmation
    {
        public Supplier Supplier { get; set; }
        public SupplierEditConfirmation()
        {
            Supplier = new Supplier();
        }
        public SupplierEditConfirmation(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
}
