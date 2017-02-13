using CompuStore.Clients.Model;
using CompuStore.Clients.Service;
using CompuStore.Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace CompuStore.Clients.ViewModels
{
    public class ClientsMainViewModel : BindableBase
    {
        #region Fields
        private readonly IRegionManager _regionManager;
        private IClientService _clientService;
        private string _searchText;
        private Client _selectedItem;
        private ObservableCollection<Client> _items;
        #endregion
        #region Properties
        public Client SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<Client> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, ()=>SelectedItem!=null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(SearchClients);
        public DelegateCommand PaymentsCommand => new DelegateCommand(ShowPayments, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SalesCommand => new DelegateCommand(ShowSales, () => SelectedItem != null).ObservesProperty(() => SelectedItem);

        private void Add()
        {
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientEdit);
        }
        private void Update()
        {
            var parameters = new NavigationParameters { { "Client", SelectedItem } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientEdit, parameters);
        }
        private void Delete()
        {
            if (_clientService.IsClientWithOrders(SelectedItem))
                Messages.Error("لايمكن حذف عميل له عمليات بيع الا بعد حذف المبيعات اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name)) return;
                _clientService.Delete(SelectedItem);
                Items.Remove(SelectedItem);
            }
        }
        private void ShowSales()
        {
            NavigationParameters parameters = new NavigationParameters { { "Client", SelectedItem } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientSalesMain,parameters);
        }

        private void ShowPayments()
        {
            NavigationParameters parameters = new NavigationParameters { { "Client", SelectedItem } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientPaymentMain,parameters);
        }

        #endregion
        private void SearchClients() => Items = new ObservableCollection<Client>(_clientService.SearchBy(SearchText));
        public ClientsMainViewModel(IClientService clientService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _searchText = "";
            _clientService = clientService;
            _regionManager = regionManager;
            eventAggregator.GetEvent<ClientAddedOrUpdated>().Subscribe(c=> SearchCommand.Execute());
            eventAggregator.GetEvent<ClientPaymentAdded>().Subscribe(RefreshClientPayment);
            eventAggregator.GetEvent<ClientPaymentUpdated>().Subscribe(RefreshClientPayment);
            eventAggregator.GetEvent<ClientPaymentDeleted>().Subscribe(RefreshClientPayment);
            Items =new ObservableCollection<Model.Client>( _clientService.GetAll());            
        }
        private void RefreshClientPayment(ClientPayment obj)
        {
            var client = Items.Single(x => x.ID == obj.ClientID);
            Client newClient = _clientService.Find(client.ID);
            DataUtils.Copy(client, newClient);
        }
    }
}
