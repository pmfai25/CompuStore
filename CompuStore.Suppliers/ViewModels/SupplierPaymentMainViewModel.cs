using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Events;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPaymentMainViewModel : BindableBase
    {
        #region Fields
        private Supplier _supplier;
        private decimal _total;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private SupplierPayment _selectedItem;
        private ObservableCollection<SupplierPayment> _items;
        private ISupplierPaymentService _supplierPaymentService;
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
            set { SetProperty(ref _dateTo, value);}
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
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);
        #endregion
        #region Methods
        private void Refresh()
        {
            if (Supplier == null)
                return;
            Items = new ObservableCollection<SupplierPayment>(_supplierPaymentService.GetAll(Supplier));
            if (Items.Count > 0)
            {
                DateFrom = Items.Min(x => x.Date).Date;
                DateTo = Items.Max(x => x.Date).Date;
            }
            Total = Items.Sum(s => s.Money);
        }
        private void Search()
        {
            if (Supplier == null)
                return;
            Items = new ObservableCollection<SupplierPayment>(_supplierPaymentService.SearchByInterval(Supplier, DateFrom, DateTo));
            Total = Items.Sum(x => x.Money);
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
            if (!Messages.Delete("فاتورة ايصال نقدية رقم " + SelectedItem.Number.ToString())) return;
            _supplierPaymentService.Delete(SelectedItem);           
            _eventAggregator.GetEvent<SupplierPaymentDeleted>().Publish(SelectedItem);
            Items.Remove(SelectedItem);
            Total = Items.Sum(x => x.Money);
        }
       
        private void OnSupplierPaymentUpdated(SupplierPayment obj)
        {
            Search();
        }

        private void OnSupplierPaymentAdded(SupplierPayment obj)
        {
            Items.Add(obj);
            Search();
        }
        #endregion
        public SupplierPaymentMainViewModel(ISupplierPaymentService supplierPaymentService, IEventAggregator eventAggregator)
        {
            DateTo = DateFrom = DateTime.Today;
            _supplierPaymentService = supplierPaymentService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SupplierPaymentAdded>().Subscribe(OnSupplierPaymentAdded);
            _eventAggregator.GetEvent<SupplierPaymentUpdated>().Subscribe(OnSupplierPaymentUpdated);
            _eventAggregator.GetEvent<SupplierSelected>().Subscribe(OnSupplierSelected);
        }

        private void OnSupplierSelected(Supplier obj)
        {
            Supplier = obj;
            Refresh();
        }
    }
}
