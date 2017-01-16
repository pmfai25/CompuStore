using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Regions;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Clients.Properties;
using System.Collections.ObjectModel;
using CompuStore.Clients.Model;
using CompuStore.Clients.Service;

namespace CompuStore.Clients.ViewModels
{
    public class ClientsMainViewModel : BindableBase
    {
        private ObservableCollection<Client> clients;
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set { SetProperty(ref clients, value); }
        }
        private Client selectedClient;
        public Client SelectedClient
        {
            get { return selectedClient; }
            set { SetProperty(ref selectedClient, value); }
        }
        private IClientService clientService;
        public string SelectedClientName {
            get {  
                    if (selectedClient != null)
                    return selectedClient.Name;
                else
                    return "Not Selected"; } }
        public ClientsMainViewModel(IClientService ClientService)
        {
            this.clientService = ClientService;
            clients = this.clientService.GetAllClients();
        }      
    }
}
