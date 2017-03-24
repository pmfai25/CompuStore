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
using Prism.Interactivity.InteractionRequest;
using CompuStore.Suppliers.Confirmations;

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
        #endregion
        #region Properties
        public InteractionRequest<SupplierPaymentConfirmation> SupplierPaymentRequest { get; set; }
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
        private void OnSupplierSelected(Supplier obj)
        {
            Supplier = obj;
            Search();
        }
        private void Refresh()
        {
            if (Supplier == null)
                return;
            Items = new ObservableCollection<SupplierPayment>(_supplierPaymentService.GetAll(Supplier));
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
            SupplierPaymentRequest.Raise(new SupplierPaymentConfirmation(Supplier.ID),
                x =>
                {
                    if (x.Confirmed)
                    {
                        _supplierPaymentService.Add(x.SupplierPayment);
                        Items.Add(x.SupplierPayment);
                        Total = Items.Sum(y => y.Money);
                    }
                });
        }
        private void Update()
        {
            SupplierPaymentRequest.Raise(new SupplierPaymentConfirmation(SelectedItem),
                x =>
                {
                    if (x.Confirmed)
                    {
                        _supplierPaymentService.Update(x.SupplierPayment);
                        Total = Items.Sum(y => y.Money);
                    }
                    else
                        DataUtils.Copy(SelectedItem, _supplierPaymentService.Find(SelectedItem.ID));
                });
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (!Messages.Delete("فاتورة ايصال نقدية رقم " + SelectedItem.Number)) return;
            _supplierPaymentService.Delete(SelectedItem);           
            Items.Remove(SelectedItem);
            Total = Items.Sum(x => x.Money);
        }        
        #endregion
        public SupplierPaymentMainViewModel(ISupplierPaymentService supplierPaymentService, IEventAggregator eventAggregator)
        {
            SupplierPaymentRequest = new InteractionRequest<SupplierPaymentConfirmation>();
            DateFrom = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
            DateTo = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day);
            _supplierPaymentService = supplierPaymentService;
            eventAggregator.GetEvent<SupplierSelected>().Subscribe(OnSupplierSelected);
        }        
    }
}
