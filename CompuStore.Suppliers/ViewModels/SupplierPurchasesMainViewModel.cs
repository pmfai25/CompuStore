using CompuStore.Infrastructure;
using CompuStore.Suppliers.Confirmations;
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

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPurchasesMainViewModel : BindableBase
    {
        #region Fields
        private ISupplierService _supplierService;
        private IPurchaseService _purchaseService;
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
        public InteractionRequest<SupplierPurchaseConfirmation> SupplierPurchaseRequest { get; set; }
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
        public DelegateCommand AddCommand => new DelegateCommand(Add,()=>Supplier!=null).ObservesProperty(()=>Supplier);
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
            SupplierPurchaseRequest.Raise(new SupplierPurchaseConfirmation(new Purchase(Supplier.ID)),
                x =>
                {
                    if (x.Confirmed)
                        Search();
                });
        }
        private void Update()
        {
            SupplierPurchaseRequest.Raise(new SupplierPurchaseConfirmation(SelectedItem, _purchaseService.GetPurchaseDetails(SelectedItem.ID)),
                x =>
                {
                    if (x.Confirmed)
                        Search();
                    else
                        DataUtils.Copy(SelectedItem, _purchaseService.FindPurchase(SelectedItem.ID));
                });
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
            SupplierPurchaseRequest = new InteractionRequest<SupplierPurchaseConfirmation>();
            DateTo = DateFrom = DateTime.Today;
            _supplierService = supplierService;
            _purchaseService = purchaseService;
            eventAggregator.GetEvent<SupplierSelected>().Subscribe(x => OnSupplierSelected(x));
        }

        
    }
}
