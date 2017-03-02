﻿using Model;
using Service;
using CompuStore.Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Events;
using Model.Views;
using System;

namespace CompuStore.Clients.ViewModels
{
    public class ClientsMainViewModel : BindableBase
    {
        #region Fields
        private Client _selectedItem;
        private string _searchText;
        private ObservableCollection<Client> _items;
        private IClientService _clientService;
        private readonly IRegionManager _regionManager;
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
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        public DelegateCommand PaymentsCommand => new DelegateCommand(ShowPayments, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SalesCommand => new DelegateCommand(ShowSales, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);
        #endregion
        #region Methods
        private void Refresh()
        {
            SearchText = "";
            Items = new ObservableCollection<Client>(_clientService.GetAll());
        }
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
            if (!_clientService.IsDeletable(SelectedItem))
                Messages.Error("لايمكن حذف عميل له عمليات بيع الا بعد حذف المبيعات اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name)) return;
                _clientService.Delete(SelectedItem);
                Items.Remove(SelectedItem);
            }
        }
        private void Search()
        {
            Items = new ObservableCollection<Client>(_clientService.SearchBy(SearchText));
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
        private void RefreshClientPayment(ClientPayment obj)
        {
            var client = Items.SingleOrDefault(x => x.ID == obj.ClientID);
            if (client == null)
                return;
            Client newClient = _clientService.Find(client.ID);
            DataUtils.Copy(client, newClient);
        }
        private void RefreshSales(Order obj)
        {
            var client = Items.SingleOrDefault(x => x.ID == obj.ClientID);
            if (client == null)
                return;
            Client newClient = _clientService.Find(client.ID);
            DataUtils.Copy(client, newClient);
        }
        #endregion

        public ClientsMainViewModel(IClientService clientService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _searchText = "";
            _regionManager = regionManager;
            _clientService = clientService;
            eventAggregator.GetEvent<ClientAdded>().Subscribe(c=> Items.Add(c));
            eventAggregator.GetEvent<ClientPaymentAdded>().Subscribe(RefreshClientPayment);
            eventAggregator.GetEvent<ClientPaymentUpdated>().Subscribe(RefreshClientPayment);
            eventAggregator.GetEvent<ClientPaymentDeleted>().Subscribe(RefreshClientPayment);
            eventAggregator.GetEvent<OrderAdded>().Subscribe(RefreshSales);
            eventAggregator.GetEvent<OrderUpdated>().Subscribe(RefreshSales);
            eventAggregator.GetEvent<OrderDeleted>().Subscribe(RefreshSales);
            Items =new ObservableCollection<Client>( _clientService.GetAll());            
        }        
    }
}
