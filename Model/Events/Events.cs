using Model.Views;
using Prism.Events;

namespace Model.Events
{
    public class ClientAdded : PubSubEvent<Client> { }
    public class ClientUpdated : PubSubEvent<Client> { }
    public class ClientDeleted : PubSubEvent<Client> { }
    public class ClientPaymentAdded : PubSubEvent<ClientPayment> { }
    public class ClientPaymentUpdated : PubSubEvent<ClientPayment> { }
    public class ClientPaymentDeleted : PubSubEvent<ClientPayment> { }
    public class SupplierAdded : PubSubEvent<Supplier> { }
    public class SupplierUpdated : PubSubEvent<Supplier> { }
    public class SupplierDeleted : PubSubEvent<Supplier> { }
    public class SupplierPaymentAdded : PubSubEvent<SupplierPayment> { }
    public class SupplierPaymentUpdated : PubSubEvent<SupplierPayment> { }
    public class SupplierPaymentDeleted : PubSubEvent<SupplierPayment> { }
    public class ItemAdded : PubSubEvent<Item> { }
    public class ItemUpdated : PubSubEvent<Item> { }
    public class ItemDeleted : PubSubEvent<Item> { }
    public class PurchaseAdded : PubSubEvent<SupplierPurchases> { }
    public class PurchaseUpdated : PubSubEvent<SupplierPurchases> { }
    public class PurchaseDeleted : PubSubEvent<SupplierPurchases> { }
    public class PurchaseItemAdded : PubSubEvent<PurchaseDetails> { }
    public class PurchaseItemUpdated : PubSubEvent<PurchaseDetails> { }
    public class PurchaseItemDeleted : PubSubEvent<PurchaseDetails> { }

    public class OrderAdded : PubSubEvent<ClientOrders> { }
    public class OrderUpdated : PubSubEvent<ClientOrders> { }
    public class OrderDeleted : PubSubEvent<ClientOrders> { }

}
