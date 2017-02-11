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
        private ClientMain _selectedItem;
        private ObservableCollection<ClientMain> _items;
        #endregion
        #region Properties
        public ClientMain SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<ClientMain> Items
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
        public DelegateCommand AddCommand => new DelegateCommand(DisplayAdd);
        public DelegateCommand UpdateCommand => new DelegateCommand(DisplayEdit, ()=>SelectedItem!=null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(DeleteClient, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(SearchClients);
        public DelegateCommand PaymentsCommand => new DelegateCommand(DisplayPayments, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SalesCommand => new DelegateCommand(DisplaySales, () => SelectedItem != null).ObservesProperty(() => SelectedItem);

        private void DisplaySales()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("ClientID", SelectedItem.ID);
            parameters.Add("ClientName", SelectedItem.Name);
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientSalesMain,parameters);
        }

        private void DisplayPayments()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("ClientID", SelectedItem.ID);
            parameters.Add("ClientName", SelectedItem.Name);
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientPaymentMain,parameters);
        }

        #endregion
        private void DisplayAdd() => _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientEdit);

        private void DisplayEdit()
        {
            var parameters = new NavigationParameters {{"ID", SelectedItem.ID}};
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientEdit,parameters);
        }

        private void DeleteClient()
        {
            if (_clientService.IsClientWithOrders(SelectedItem.ID))
                Messages.Error("لايمكن حذف عميل له عمليات شراء الا بعد حذف عمليات الشراء اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name)) return;
                _clientService.Delete(new Client() { ID = SelectedItem.ID });
                Items.Remove(SelectedItem);
            }
        }
       
        private void SearchClients() => Items = new ObservableCollection<ClientMain>(_clientService.SearchBy(SearchText));

        public ClientsMainViewModel(IClientService clientService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _clientService = clientService;
            _regionManager = regionManager;
            eventAggregator.GetEvent<ClientAddedOrUpdated>().Subscribe(c=> SearchCommand.Execute());
            Items = new ObservableCollection<ClientMain>();
            _searchText = "";
        }
    }
}
