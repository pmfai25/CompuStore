using Model.Views;
using Prism.Events;

namespace Model.Events
{
    public class ClientAdded : PubSubEvent<Client> { }
    public class ClientUpdated : PubSubEvent<Client> { }
    public class ClientPaymentAdded : PubSubEvent<ClientPayment> { }
    public class ClientPaymentUpdated : PubSubEvent<ClientPayment> { }
    public class ClientPaymentDeleted : PubSubEvent<ClientPayment> { }
    public class SupplierAdded : PubSubEvent<Supplier> { }
    public class SupplierUpdated : PubSubEvent<Supplier> { }
    public class SupplierPaymentAdded : PubSubEvent<SupplierPayment> { }
    public class SupplierPaymentUpdated : PubSubEvent<SupplierPayment> { }
    public class SupplierPaymentDeleted : PubSubEvent<SupplierPayment> { }    
    public class ItemAdded : PubSubEvent<Item> { }
    public class ItemUpdated : PubSubEvent<Item> { }
    public class PurchaseAdded : PubSubEvent<Purchase> { }
    public class PurchaseUpdated : PubSubEvent<Purchase> { }
    public class PurchaseDeleted : PubSubEvent<Purchase> { }
    public class OrderAdded : PubSubEvent<Order> { }
    public class OrderUpdated : PubSubEvent<Order> { }
    public class OrderDeleted : PubSubEvent<Order> { }

}
