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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPurchasesMainViewModel : BindableBase, INavigationAware,IRegionMemberLifetime
    {
        #region Fields
        private ISupplierService _supplierService;
        private IPurchaseService _purchaseService;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private NavigationContext _navigationContext;
        private Supplier _supplier;
        private Purchase selectedItem;
        private ObservableCollection<Purchase> items;
        private decimal total;
        private DateTime dateFrom;
        private DateTime dateTo;
        private ObservableCollection<PurchaseDetails> _details;
        #endregion
        #region Properties
        public ObservableCollection<PurchaseDetails> Details
        {
            get { return _details; }
            set { SetProperty(ref _details, value); }
        }
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
        public decimal Total
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }
        public Purchase SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }
        public ObservableCollection<Purchase> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);
        public DelegateCommand BackCommand => new DelegateCommand(Back);
        public DelegateCommand SelectedItemChangedCommand => new DelegateCommand(SelectedItemChanged);

        private void Add()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Supplier", Supplier);
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierPurchaseEdit,parameters);
        }
        private void Update()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Purchase", SelectedItem);
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierPurchaseEdit, parameters);
        }
        private void Delete()
        {
            if (Messages.Delete("فاتورة رقم " + selectedItem.Number))
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
            }
            else
                DateFrom = DateTo = DateTime.Today;
        }
        private void Back()
        {
            _navigationContext.NavigationService.Journal.GoBack();
        }
        private void SelectedItemChanged()
        {
            if (SelectedItem != null)
                Details = new ObservableCollection<PurchaseDetails>(_purchaseService.GetPurchaseDetails(SelectedItem.ID));
            else
                Details = null;
        }
        #endregion
        #region Events
        private void OnPurchaseUpdated(Purchase obj)
        {
            Search();
        }        
        private void OnPurchaseAdded(Purchase obj)
        {
            Items.Add(obj);
            Search();
        }
        #endregion
        #region Interface
        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Supplier = (Supplier)navigationContext.Parameters["Supplier"];
            Refresh();
            _navigationContext = navigationContext;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
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
            eventAggregator.GetEvent<PurchaseAdded>().Subscribe(OnPurchaseAdded);
            eventAggregator.GetEvent<PurchaseUpdated>().Subscribe(OnPurchaseUpdated);
            Supplier = new Supplier();
        }
    }
}
