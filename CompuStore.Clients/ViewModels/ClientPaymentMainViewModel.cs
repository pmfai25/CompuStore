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

namespace CompuStore.Clients.ViewModels
{
    public class ClientPaymentMainViewModel : BindableBase, INavigationAware
    {
        #region Fields
        private Client _client;        
        private decimal _total;
        private DateTime _dateFrom;       
        private DateTime _dateTo;       
        private ClientPayment _selectedItem;        
        private ObservableCollection<ClientPayment> _items;        
        private IClientPaymentService _clientPaymentService;
        private NavigationContext _navigationContext;
        private IEventAggregator _eventAggregator;
        #endregion
        #region Properties
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
        public DelegateCommand BackCommand => new DelegateCommand(Back);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        #endregion
        #region Methods

        private void Back()
        {
            if(_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }
        private void Add()
        {
            var parameters = new NavigationParameters { { "Client", _client } };
            _navigationContext.NavigationService.RequestNavigate( RegionNames.ClientPaymentEdit, parameters);
        }
        private void Update()
        {
            var parameters = new NavigationParameters { { "ClientPayment", SelectedItem } };
            _navigationContext.NavigationService.RequestNavigate(RegionNames.ClientPaymentEdit, parameters);
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (!Messages.Delete("فاتورة ايصال نقدية رقم " + SelectedItem.Number.ToString())) return;
            if(!_clientPaymentService.Delete(SelectedItem))
            {
                Messages.ErrorDataNotSaved();
                return;
            }
            _eventAggregator.GetEvent<ClientPaymentDeleted>().Publish(SelectedItem);
            Items.Remove(SelectedItem);
            Total = Items.Sum(x => x.Money);            
        }
        private void Search()
        {
            Items = new ObservableCollection<ClientPayment>( _clientPaymentService.SearchByInterval(_client, DateFrom, DateTo));
            Total = Items.Sum(x => x.Money);
        }
        #endregion
        #region Interfaces
        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            var c2 = (Client)(navigationContext.Parameters["Client"]);
            DataUtils.Copy(_client, c2);
            Items = new ObservableCollection<ClientPayment>(_clientPaymentService.GetAll(_client));
            Total = Items.Sum(s => s.Money);            
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            var c2= (Client)(navigationContext.Parameters["Client"]);
            return c2.ID == _client.ID;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
        public ClientPaymentMainViewModel(IClientPaymentService clientPaymentService,IRegionManager regionManager,IEventAggregator eventAggregator)
        {
            Items = new ObservableCollection<Model.ClientPayment>();
            _client = new Client();
            _clientPaymentService = clientPaymentService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ClientPaymentAdded>().Subscribe(OnClientAdded);
            _eventAggregator.GetEvent<ClientPaymentUpdated>().Subscribe(OnClientPaymentUpdated);
            DateTo = DateTime.Today;
            DateFrom = DateTime.Today;
        }
        private void OnClientPaymentUpdated(ClientPayment obj)
        {
            Total = Items.Sum(i => i.Money);
        }

        private void OnClientAdded(ClientPayment obj)
        {
            if (obj.Date <= DateTo && obj.Date >= DateFrom)
            {
                Items.Add(obj);
                Total = Items.Sum(i => i.Money);
            }
        }
    }
}
