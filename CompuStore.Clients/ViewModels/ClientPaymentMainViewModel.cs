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

namespace CompuStore.Clients.ViewModels
{
    public class ClientPaymentMainViewModel : BindableBase, INavigationAware
    {
        #region Fields
        private decimal _total;
        private int clientID;
        private DateTime _dateFrom;       
        private DateTime _dateTo;       
        private string title;       
        private ClientPayment _selectedItem;        
        private ObservableCollection<ClientPayment> _items;        
        private IClientPaymentService _clientPaymentService;
        private IRegionManager _regionManager;
        private NavigationContext _navigationContext;
        #endregion
        #region Properties
        public decimal Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
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
            var parameters = new NavigationParameters();
            parameters.Add("ClientID", clientID);
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientPaymentEdit, parameters);
        }
        private void Update()
        {
            var parameters = new NavigationParameters();
            parameters.Add("ClientID", clientID);
            parameters.Add("ClientPaymentID", SelectedItem.ID);
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientPaymentEdit, parameters);
        }
        private void Delete()
        {
            if (!Messages.Delete("فاتورة الشراء رقم " + SelectedItem.Number.ToString())) return;
            _clientPaymentService.Delete(SelectedItem);
            Items.Remove(SelectedItem);
            Total = Items.Sum(x => x.Money);
        }
        private void Search()
        {
            Items = new ObservableCollection<ClientPayment>( _clientPaymentService.SearchByInterval(DateFrom, DateTo));
            Total = Items.Sum(x => x.Money);
        }
        #endregion
        #region Interfaces
        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            clientID = (int)(navigationContext.Parameters["ClientID"]);
            Title = (string)(navigationContext.Parameters["ClientName"]);
            DateTo = DateTime.Now;
            DateFrom = DateTime.Now;
            _navigationContext = navigationContext;
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
        public ClientPaymentMainViewModel(IClientPaymentService clientPaymentService,IRegionManager regionManager,IEventAggregator eventAggregator)
        {
            _clientPaymentService = clientPaymentService;
            _regionManager = regionManager;            
            eventAggregator.GetEvent<ClientPaymentAdded>().Subscribe(c => SearchCommand.Execute());
            eventAggregator.GetEvent<ClientPaymentUpdated>().Subscribe(c => SearchCommand.Execute());
            eventAggregator.GetEvent<ClientPaymentDeleted>().Subscribe(c => SearchCommand.Execute());
        }
    }
}
