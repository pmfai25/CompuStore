using Model;
using Service;
using CompuStore.Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Events;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Clients.Confirmations;

namespace CompuStore.Clients.ViewModels
{
    public class ClientPaymentMainViewModel : BindableBase
    {
        #region Fields
        private Client _client;        
        private decimal _total;
        private DateTime _dateFrom;       
        private DateTime _dateTo;       
        private ClientPayment _selectedItem;        
        private ObservableCollection<ClientPayment> _items;        
        private IClientPaymentService _clientPaymentService;
        #endregion
        #region Properties
        public InteractionRequest<ClientPaymentConfirmation> ClientPaymentRequest { get; set; }
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
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }
        public ClientPayment SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<ClientPayment> Items
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
        private void OnClientSelected(Client obj)
        {
            Client = obj;
            Refresh();
        }
        private void Refresh()
        {
            if (Client == null)
                return;
            Items = new ObservableCollection<ClientPayment>(_clientPaymentService.GetAll(Client));
            if (Items.Count > 0)
            {
                DateFrom = Items.Min(x => x.Date).Date;
                DateTo = Items.Max(x => x.Date).Date;
            }
            Total = Items.Sum(s => s.Money);
        }
        private void Search()
        {
            if (Client == null)
                return;
            Items = new ObservableCollection<ClientPayment>(_clientPaymentService.SearchByInterval(_client, DateFrom, DateTo));
            Total = Items.Sum(x => x.Money);
        }
        private void Add()
        {
            ClientPaymentRequest.Raise(new ClientPaymentConfirmation(Client.ID), x =>
             {
                 if(x.Confirmed)
                 {
                     _clientPaymentService.Add(x.ClientPayment);
                     Search();
                 }
             });
        }
        private void Update()
        {
            ClientPaymentRequest.Raise(new ClientPaymentConfirmation(SelectedItem), x =>
             {
                 if (x.Confirmed)
                 {
                     _clientPaymentService.Update(x.ClientPayment);
                     Search();
                 }
                 else
                     DataUtils.Copy(SelectedItem, _clientPaymentService.Find(SelectedItem.ID));
             });
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (!Messages.Delete("فاتورة ايصال نقدية رقم " + SelectedItem.Number.ToString())) return;
            _clientPaymentService.Delete(SelectedItem);
            Items.Remove(SelectedItem);
            Total = Items.Sum(x => x.Money);            
        }        
        #endregion
        public ClientPaymentMainViewModel(IClientPaymentService clientPaymentService,IEventAggregator eventAggregator)
        {
            ClientPaymentRequest = new InteractionRequest<ClientPaymentConfirmation>();
            _clientPaymentService = clientPaymentService;
            eventAggregator.GetEvent<ClientSelected>().Subscribe(OnClientSelected);
            DateTo = DateFrom = DateTime.Today;
        }      
    }
}
