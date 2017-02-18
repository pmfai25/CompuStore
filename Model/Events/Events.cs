﻿using Prism.Events;

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

}