using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using Model.Views;
using Model;
using System.Collections.ObjectModel;
using Service;

namespace CompuStore.Clients.ViewModels
{
    public class ClientSalesMainViewModel : BindableBase,INavigationAware
    {
        #region Fields
        private DateTime dateTo;
        private DateTime dateFrom;
        private Client _client;        
        private ClientOrders _selectedOrder;
        private ObservableCollection<OrderDetails> _details;
        private ObservableCollection<ClientOrders> _orders;
        private IOrderService _orderService;
        private NavigationContext _navigationContext;
        #endregion
        #region Properties             
        public DateTime DateTo
        {
            get { return dateTo; }
            set { SetProperty(ref dateTo, value); }
        }       
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { SetProperty(ref dateFrom, value); }
        }
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
        public ClientOrders SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (value == null)
                    Details = null;
                else
                    if (_selectedOrder!=null&& value.OrderID != _selectedOrder.OrderID)
                    Details = new ObservableCollection<OrderDetails>(_orderService.GetOrderDetails(value));
                SetProperty(ref _selectedOrder, value);
            }
        }
        public ObservableCollection<ClientOrders> Orders
        {
            get { return _orders; }
            set { SetProperty(ref _orders, value); }
        }
        public ObservableCollection<OrderDetails> Details
        {
            get { return _details; }
            set { SetProperty(ref _details, value); }
        }
        public DelegateCommand BackCommand => new DelegateCommand(Back);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);

        private void Search()
        {
            Orders = new ObservableCollection<ClientOrders>(_orderService.GetClientOrders(Client, DateFrom, DateTo));
        }
        #endregion
        private void Back()
        {
            if (_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }

        public ClientSalesMainViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            Client = new Client();
            SelectedOrder = new ClientOrders();
            Orders = new ObservableCollection<ClientOrders>();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            DateTo = DateFrom = DateTime.Today;
            Client = (Client)_navigationContext.Parameters["Client"];
            Orders =new ObservableCollection<ClientOrders>( _orderService.GetClientOrders(Client));
            SelectedOrder = Orders.FirstOrDefault();
        }
    }
}
