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
    public class SupplierPurchasesMainViewModel : BindableBase
    {
        #region Fields
        private ISupplierService _supplierService;
        private IPurchaseService _purchaseService;
        private IEventAggregator _eventAggregator;
        private Supplier _supplier;
        private Purchase _selectedItem;
        private decimal _total;
        private decimal _paid;       
        private decimal _remaining;        
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private ObservableCollection<Purchase> _items;
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
        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
        }
        public Purchase SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<Purchase> Items
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
        private void OnSupplierSelected(Supplier x)
        {
            Supplier = x;
            Refresh();
        }
        private void Add()
        {

        }
        private void Update()
        {

        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if(!_purchaseService.IsDeletable(SelectedItem))
            {
                Messages.Error("لا يمكن حذف هذة الفاتورة لانه توجد مبيعات مرتبطة بالاصناف التي تم شراءها في هذة الفاتورة");
                return;
            }
            if (Messages.Delete("فاتورة رقم " + _selectedItem.Number))
            {
                _purchaseService.DeletePurchase( SelectedItem);
                _eventAggregator.GetEvent<PurchaseDeleted>().Publish(SelectedItem);
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
            if (Supplier == null)
                return;
            Items = new ObservableCollection<Purchase>(_supplierService.GetPurchases(Supplier, DateFrom, DateTo));
            FixData();
        }
        private void Refresh()
        {
            Total = Paid = Remaining = 0;
            if (Supplier == null)
                return;
            Items = new ObservableCollection<Purchase>(_supplierService.GetPurchases(Supplier));
            if (Items.Count > 0)
            {
                DateFrom = Items.Min(x => x.Date).Date;
                DateTo = Items.Max(x => x.Date).Date;
                FixData();
                SelectedItem = Items.First();
            }
        }
        #endregion
        public SupplierPurchasesMainViewModel(ISupplierService supplierService, IPurchaseService purchaseService, IEventAggregator eventAggregator)
        {
            DateTo = DateFrom = DateTime.Today;
            _supplierService = supplierService;
            _purchaseService = purchaseService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PurchaseAdded>().Subscribe(x=>Search());
            _eventAggregator.GetEvent<PurchaseUpdated>().Subscribe(x => Search());
            _eventAggregator.GetEvent<SupplierSelected>().Subscribe(x => OnSupplierSelected(x));
        }

        
    }
}
