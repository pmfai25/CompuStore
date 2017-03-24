using Model;
using Model.Views;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Clients.Confirmations
{
    public class ClientSaleConfirmation:Confirmation
    {
        public Orders ClientOrder { get; set; }
        public ObservableCollection<OrderDetails> Details { get; set; }
        public ClientSaleConfirmation(int clientID)
        {
            Title = "";
            ClientOrder = new Orders(clientID);
            Details = new ObservableCollection<OrderDetails>();
            ClientOrder.ClientID = clientID;
        }
        public ClientSaleConfirmation(Orders clientOrder,IEnumerable<OrderDetails> orderDetails)
        {
            Title = "";
            ClientOrder = clientOrder;
            Details = new ObservableCollection<OrderDetails>(orderDetails);
        }

    }
}
