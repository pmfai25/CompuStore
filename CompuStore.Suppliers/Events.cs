using CompuStore.Suppliers.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Suppliers
{
    public class SupplierAdded : PubSubEvent<Supplier> { }
    public class SupplierUpdated : PubSubEvent<Supplier> { }
    public class SupplierPaymentAdded : PubSubEvent<SupplierPayment> { }
    public class SupplierPaymentUpdated : PubSubEvent<SupplierPayment> { }
    public class SupplierPaymentDeleted : PubSubEvent<SupplierPayment> { }
}
