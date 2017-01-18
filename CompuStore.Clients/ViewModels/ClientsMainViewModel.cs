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
            set {
               SetProperty(ref selectedClient, value);

            }
        }
        private IClientService clientService;
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand EditCommand => new DelegateCommand(Edit, CanEdit).ObservesProperty(()=>SelectedClient);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, CanDelete).ObservesProperty(() => SelectedClient);

        private void Delete()
        {
            Clients.Remove(SelectedClient);
        }

        private bool CanDelete()
        {
            return SelectedClient != null;
        }

        private void Edit()
        {
            
        }

        private bool CanEdit()
        {
            return SelectedClient != null;
        }

        private void Add()
        {
            
        }
        public ClientsMainViewModel(IClientService ClientService)
        {
            this.clientService = ClientService;
            clients = this.clientService.GetAllClients();
        }      
    }
}
