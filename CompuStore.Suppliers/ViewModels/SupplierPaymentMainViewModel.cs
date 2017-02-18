using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Events;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPaymentMainViewModel : BindableBase,INavigationAware
    {
        #region Fields
        private Supplier _supplier;

        private decimal _total;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private SupplierPayment _selectedItem;
        private ObservableCollection<SupplierPayment> _items;
        private ISupplierPaymentService _supplierPaymentService;
        private IRegionManager _regionManager;
        private NavigationContext _navigationContext;
        private IEventAggregator _eventAggregator;
        #endregion
        #region Properties
        public decimal Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
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
        public SupplierPayment SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<SupplierPayment> Items
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
            if (_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }
        private void Add()
        {
            var parameters = new NavigationParameters { { "Supplier", _supplier } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierPaymentEdit, parameters);
        }
        private void Update()
        {
            var parameters = new NavigationParameters { { "SupplierPayment", SelectedItem } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierPaymentEdit, parameters);
        }
        private void Delete()
        {
            if (!Messages.Delete("فاتورة ايصال نقدية رقم " + SelectedItem.Number.ToString())) return;
            if (!_supplierPaymentService.Delete(SelectedItem))
            {
                Messages.ErrorDataNotSaved();
                return;
            }
            _eventAggregator.GetEvent<SupplierPaymentDeleted>().Publish(SelectedItem);
            Items.Remove(SelectedItem);
            Total = Items.Sum(x => x.Money);
        }
        private void Search()
        {
            Items = new ObservableCollection<SupplierPayment>(_supplierPaymentService.SearchByInterval(_supplier, DateFrom, DateTo));
            Total = Items.Sum(x => x.Money);
        }
        #endregion
        #region Interfaces
        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            var c2 = (Supplier)(navigationContext.Parameters["Supplier"]);
            DataUtils.Copy(_supplier, c2);
            Items = new ObservableCollection<Model.SupplierPayment>(_supplierPaymentService.GetAll(_supplier));
            Total = Items.Sum(s => s.Money);
            _navigationContext = navigationContext;
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            var c2 = (Supplier)(navigationContext.Parameters["Supplier"]);
            return c2.ID == _supplier.ID;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
        public SupplierPaymentMainViewModel(ISupplierPaymentService supplierPaymentService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _supplier = new Supplier();
            _supplierPaymentService = supplierPaymentService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SupplierPaymentAdded>().Subscribe(OnSupplierPaymentAdded);
            _eventAggregator.GetEvent<SupplierPaymentUpdated>().Subscribe(OnSupplierPaymentUpdated);
            DateTo = DateTime.Today;
            DateFrom = DateTime.Today;
        }

        private void OnSupplierPaymentUpdated(SupplierPayment obj)
        {
            Total = Items.Sum(i => i.Money);
        }

        private void OnSupplierPaymentAdded(SupplierPayment obj)
        {
            Items.Add(obj);
            Total = Items.Sum(i => i.Money);
        }
    }
}
