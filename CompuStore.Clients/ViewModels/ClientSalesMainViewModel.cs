using CompuStore.Infrastructure;
using Model;
using Model.Events;
using Model.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Clients.ViewModels
{
    public class ClientSalesMainViewModel : BindableBase,INavigationAware
    {
        #region Fields        
        private IOrderService _orderService;
        private IClientService _clientService;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private NavigationContext _navigationContext;
        private Client _client;
        private Orders _selectedItem;
        private decimal _total;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private ObservableCollection<Orders> _items;
        private ObservableCollection<OrderDetails> _details;
        #endregion
        #region Properties             
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }       
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }
        public decimal Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
        public Orders SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                if (_selectedItem != null)
                    Details = new ObservableCollection<OrderDetails>(_orderService.GetOrderDetails(value));
                else
                    Details = null;                
            }
        }
        public ObservableCollection<Orders> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public ObservableCollection<OrderDetails> Details
        {
            get { return _details; }
            set { SetProperty(ref _details, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);
        public DelegateCommand BackCommand => new DelegateCommand(Back);
        #endregion
        #region Methods
        private void Add()
        {
            NavigationParameters parameters = new NavigationParameters { { "Client", Client } };
            _navigationContext.NavigationService.RequestNavigate( RegionNames.ClientSaleEdit, parameters);
        }
        private void Update()
        {
            NavigationParameters parameters = new NavigationParameters { { "Order", SelectedItem }, { "Client", Client }, { "Details", Details } };
            _navigationContext.NavigationService.RequestNavigate( RegionNames.ClientSaleEdit, parameters);
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (Messages.Delete("فاتورة رقم " + _selectedItem.Number))
            {
                _orderService.DeleteOrder(SelectedItem);
                _eventAggregator.GetEvent<OrderDeleted>().Publish(SelectedItem);
                Items.Remove(SelectedItem);
                Total = Items.Sum(i => i.Total);                
            }
        }
        private void Search()
        {
            Items = new ObservableCollection<Orders>(_clientService.GetOrders(Client, DateFrom, DateTo));
            SelectedItem = Items.FirstOrDefault();
            Total = Items.Sum(x => x.Total);
        }
        private void Refresh()
        {
            Items = new ObservableCollection<Orders>(_clientService.GetOrders(Client));
            if (Items.Count > 0)
            {
                DateFrom = Items.Min(x => x.Date).Date;
                DateTo = Items.Max(x => x.Date).Date;
                Total = Items.Sum(x => x.Total);
                SelectedItem = Items.First();
            }
            else
                DateFrom = DateTo = DateTime.Today;
        }
        private void Back()
        {
            _navigationContext.NavigationService.RequestNavigate(RegionNames.ClientsMain);
        }

        #endregion
        #region Interface
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var c2 = (Client)navigationContext.Parameters["Client"];
            return c2.ID == Client.ID;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Client = (Client)_navigationContext.Parameters["Client"];
            Refresh();
        }
        #endregion
        public ClientSalesMainViewModel(IClientService clientService, IOrderService orderService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _orderService = orderService;
            _clientService = clientService;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _eventAggregator.GetEvent<OrderAdded>().Subscribe(x => Search());
            _eventAggregator.GetEvent<OrderUpdated>().Subscribe(x => Search());
        }
    }
}
