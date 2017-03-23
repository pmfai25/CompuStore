using Model;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Clients.Confirmations
{
    public class ClientConfirmation:Confirmation
    {
        public Client Client { get; set; }
        public ClientConfirmation()
        {
            Title = "";
            Client = new Client();
        }
        public ClientConfirmation(Client client)
        {
            Title = "";
            Client = client;
        }
    }
}
