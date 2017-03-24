using System;
using Model.Views;
using Prism.Events;

namespace Model.Events
{
    public class RegisterValues
    {
        public string Challenge { get; set; }
        public string Serial { get; set; }
    }
    public class DoRegister : PubSubEvent<RegisterValues> { }
    public class DoLogin : PubSubEvent { }
    public class NormalUserLoggedIn : PubSubEvent{ }
    public class SerialValid : PubSubEvent<string> { }
    public class ClientAdded : PubSubEvent<Client> { }
    public class ClientUpdated : PubSubEvent<Client> { }
    public class ClientPaymentAdded : PubSubEvent<ClientPayment> { }
    public class ClientPaymentUpdated : PubSubEvent<ClientPayment> { }
    public class ClientPaymentDeleted : PubSubEvent<ClientPayment> { }
    public class ClientSelected : PubSubEvent<Client> { }
    public class SupplierSelected : PubSubEvent<Supplier>{ }
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
    public class OrderAdded : PubSubEvent<Orders> { }
    public class OrderUpdated : PubSubEvent<Orders> { }
    public class OrderDeleted : PubSubEvent<Orders> { }

}
