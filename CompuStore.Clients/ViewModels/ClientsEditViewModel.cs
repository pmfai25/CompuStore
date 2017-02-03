using CompuStore.Clients.Model;
using CompuStore.Clients.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Clients.ViewModels
{
    public class ClientsEditViewModel : BindableBase
    {
        private Client client;
        public string PropertyName
        {
            get { return client; }
            set { SetProperty(ref client, value); }
        }
        public ClientsEditViewModel(IClientService clientService)
        {

        }
    }
}
