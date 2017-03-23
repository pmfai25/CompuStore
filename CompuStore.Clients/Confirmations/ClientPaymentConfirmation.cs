using Model;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Clients.Confirmations
{
    public class ClientPaymentConfirmation:Confirmation
    {
        public ClientPayment ClientPayment { get; set; }
        public ClientPaymentConfirmation(int clientID)
        {
            Title = "";
            ClientPayment = new ClientPayment();
            ClientPayment.ClientID = clientID;
        }
        public ClientPaymentConfirmation(ClientPayment clientPayment)
        {
            Title = "";
            ClientPayment = clientPayment;
        }
    }
}
