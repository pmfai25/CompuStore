using CompuStore.Clients.Confirmations;
using CompuStore.Infrastructure;
using Model;
using Model.Events;
using Model.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Clients.ViewModels
{
    public class ClientSalesMainViewModel : BindableBase
    {
        #region Fields        
        private IOrderService _orderService;
        private IClientService _clientService;
        private Client _client;
        private Orders _selectedItem;
        private decimal _total;
        private decimal _paid;
        private decimal _remaining;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private ObservableCollection<Orders> _items;
        #endregion
        #region Properties             
        public InteractionRequest<ClientSaleConfirmation> ClientSaleRequest { get; set; }
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
        public decimal Paid
        {
            get { return _paid; }
            set { SetProperty(ref _paid, value); }
        }
        public decimal Remaining
        {
            get { return _remaining; }
            set { SetProperty(ref _remaining, value); }
        }
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
        public Orders SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<Orders> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);
        #endregion
        #region Methods
        private void OnClientSelected(Client x)
        {
            Client = x;
            Search();
        }
        private void Add()
        {
            ClientSaleRequest.Raise(new ClientSaleConfirmation(Client.ID), x =>
             {
                 if (x.Confirmed)
                 {
                     Items.Add(x.ClientOrder);
                     FixData();
                 }
             });
        }
        private void Update()
        {
            ClientSaleRequest.Raise(new ClientSaleConfirmation(SelectedItem, _orderService.GetOrderDetails(SelectedItem)), x =>
            {
                if (!x.Confirmed)
                    DataUtils.Copy(SelectedItem, _orderService.FindOrder(SelectedItem.ID));
                else
                    FixData();
            });
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (Messages.Delete("فاتورة رقم " + _selectedItem.Number))
            {
                _orderService.DeleteOrder(SelectedItem);
                Items.Remove(SelectedItem);
                FixData();              
            }
        }

        private void FixData()
        {
            Total = Items.Sum(x => x.Total);
            Paid = Items.Sum(x => x.Paid);
            Remaining = Total - Paid;
        }

        private void Search()
        {
            Total = Paid = Remaining = 0;
            if (Client == null)
                return;
            Items = new ObservableCollection<Orders>(_clientService.GetOrders(Client, DateFrom, DateTo));
            SelectedItem = Items.FirstOrDefault();
            FixData();
        }
        private void Refresh()
        {
            Total = Paid = Remaining = 0;
            if (Client == null)
                return;
            Items = new ObservableCollection<Orders>(_clientService.GetOrders(Client));
            FixData();
        }
        #endregion
        public ClientSalesMainViewModel(IClientService clientService, IOrderService orderService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            ClientSaleRequest = new InteractionRequest<ClientSaleConfirmation>();
            DateFrom = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
            DateTo = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day);
            _orderService = orderService;
            _clientService = clientService;
            eventAggregator.GetEvent<ClientSelected>().Subscribe(OnClientSelected);
        }
    }
}
