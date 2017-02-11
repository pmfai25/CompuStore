using CompuStore.Clients.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Clients
{
    public class ClientAddedOrUpdated : PubSubEvent<Client> { }
    public class ClientPaymentAdded : PubSubEvent<ClientPayment> { }
    public class ClientPaymentUpdated : PubSubEvent<ClientPayment> { }
    public class ClientPaymentDeleted : PubSubEvent<ClientPayment> { }
}
