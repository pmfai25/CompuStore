using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Events;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Suppliers.Confirmations;

namespace CompuStore.Suppliers.ViewModels
{
    public class SuppliersMainViewModel : BindableBase
    {
        #region Fields
        private Supplier _selectedItem;
        private string _searchText;
        private ObservableCollection<Supplier> items;
        private readonly ISupplierService _supplierService;
        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        #endregion
        #region Properties
        public InteractionRequest<SupplierEditConfirmation> SupplierEditRequest { get; set; }
        public Supplier SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value);_eventAggregator.GetEvent<SupplierSelected>().Publish(SelectedItem); }
        }
        public ObservableCollection<Supplier> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); SelectedItem = Items.FirstOrDefault(); }
        }
        public string SearchText
        {
            get { return _searchText; }
            set {
                SetProperty(ref _searchText, value);
            }
        }
        #endregion
        #region Commands
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        #endregion
        #region Methods
        private void Add()
        {
            SupplierEditConfirmation confirmation = new Confirmations.SupplierEditConfirmation();
            confirmation.Title = "اضافة مورد جديد";
            SupplierEditRequest.Raise(confirmation, x =>
            {
                if(x.Confirmed)
                {
                    Items.Add(x.Supplier);
                    _supplierService.Add(x.Supplier);
                }
            });
        }
        private void Update()
        {
            SupplierEditConfirmation confirmation = new Confirmations.SupplierEditConfirmation();
            confirmation.Title = "تعديل مورد";
            confirmation.Supplier = SelectedItem;
            SupplierEditRequest.Raise(confirmation, x =>
            {
                if (x.Confirmed)
                    _supplierService.Update(x.Supplier);
                else
                    DataUtils.Copy(SelectedItem, _supplierService.Find(SelectedItem.ID));
            });
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (!_supplierService.IsDeleteable(SelectedItem))
                Messages.Error("لايمكن حذف شركة لها عمليات شراء الا بعد حذف المشتريات اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name)) return;
                _supplierService.Delete(SelectedItem);
                Items.Remove(SelectedItem);

            }
        }
        private void Search()
        {
            Items = new ObservableCollection<Supplier>(_supplierService.SearchBy(SearchText));
        }
        private void RefreshSupplierPayments(SupplierPayment obj)
        {
            var supplier = Items.SingleOrDefault(x => x.ID == obj.SupplierID);
            if (supplier == null)
                return;
            Supplier newSupplier = _supplierService.Find(supplier.ID);
            DataUtils.Copy(supplier, newSupplier);
        }
        private void RefreshPurchases(Purchase obj)
        {
            var supplier = Items.SingleOrDefault(x => x.ID == obj.SupplierID);
            if (supplier == null)
                return;
            Supplier newSupplier = _supplierService.Find(supplier.ID);
            DataUtils.Copy(supplier, newSupplier);
        }
        #endregion
        public SuppliersMainViewModel(ISupplierService supplierService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            SupplierEditRequest = new InteractionRequest<Confirmations.SupplierEditConfirmation>();
            _searchText = "";
            _regionManager = regionManager;
            _supplierService = supplierService;
            eventAggregator.GetEvent<SupplierAdded>().Subscribe(x=>Items.Add(x));
            eventAggregator.GetEvent<SupplierPaymentAdded>().Subscribe(RefreshSupplierPayments);
            eventAggregator.GetEvent<SupplierPaymentUpdated>().Subscribe(RefreshSupplierPayments);
            eventAggregator.GetEvent<SupplierPaymentDeleted>().Subscribe(RefreshSupplierPayments);
            eventAggregator.GetEvent<PurchaseAdded>().Subscribe(RefreshPurchases);
            eventAggregator.GetEvent<PurchaseUpdated>().Subscribe(RefreshPurchases);
            eventAggregator.GetEvent<PurchaseDeleted>().Subscribe(RefreshPurchases);
            Items = new ObservableCollection<Supplier>(_supplierService.GetAll());
            SelectedItem = Items.FirstOrDefault();
        }
       
    }
}
