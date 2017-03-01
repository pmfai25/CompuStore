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

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPurchasesMainViewModel : BindableBase, INavigationAware
    {
        #region Fields
        private ISupplierService _supplierService;
        private IPurchaseService _purchaseService;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private NavigationContext _navigationContext;
        private Supplier _supplier;
        private Purchase _selectedItem;
        private decimal _total;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private ObservableCollection<Purchase> _items;
        private ObservableCollection<PurchaseDetails> _details;
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
        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
        }
        public Purchase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                if (_selectedItem != null)
                    Details = new ObservableCollection<PurchaseDetails>(_purchaseService.GetPurchaseDetails(_selectedItem.ID));
                else
                    Details = null;
            }
        }
        public ObservableCollection<Purchase> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public ObservableCollection<PurchaseDetails> Details
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
            NavigationParameters parameters = new NavigationParameters { { "Supplier", Supplier } };
            _navigationContext.NavigationService.RequestNavigate(RegionNames.SupplierPurchaseEdit,parameters);
        }
        private void Update()
        {
            NavigationParameters parameters = new NavigationParameters { { "Purchase", SelectedItem }, { "Supplier", Supplier }, { "Details", Details } };
            _navigationContext.NavigationService.RequestNavigate(RegionNames.SupplierPurchaseEdit, parameters);
        }
        private void Delete()
        {
            if (Messages.Delete("فاتورة رقم " + _selectedItem.Number))
            {
                _purchaseService.DeletePurchase( SelectedItem);
                Items.Remove(SelectedItem);
                Total = Items.Sum(i => i.Total);
                _eventAggregator.GetEvent<PurchaseDeleted>().Publish(SelectedItem);
            }
        }
        private void Search()
        {
            Items = new ObservableCollection<Purchase>(_supplierService.GetPurchases(Supplier, DateFrom, DateTo));
            Total = Items.Sum(x => x.Total);            
        }
        private void Refresh()
        {
            Items = new ObservableCollection<Purchase>(_supplierService.GetPurchases(Supplier));
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
            _navigationContext.NavigationService.RequestNavigate(RegionNames.SuppliersMain);
        }
        #endregion
        #region Interface
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Supplier = (Supplier)navigationContext.Parameters["Supplier"];
            Refresh();            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var s2 = (Supplier)navigationContext.Parameters["Supplier"];
            return s2.ID == Supplier.ID;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        #endregion
        public SupplierPurchasesMainViewModel(ISupplierService supplierService, IPurchaseService purchaseService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _supplierService = supplierService;
            _purchaseService = purchaseService;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _eventAggregator.GetEvent<PurchaseAdded>().Subscribe(x=>Search());
            _eventAggregator.GetEvent<PurchaseUpdated>().Subscribe(x => Search());
        }
    }
}
