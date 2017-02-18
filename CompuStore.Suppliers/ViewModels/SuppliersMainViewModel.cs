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

namespace CompuStore.Suppliers.ViewModels
{
    public class SuppliersMainViewModel : BindableBase
    {
        private Supplier _selectedItem;
        private string _searchText;
        private ObservableCollection<Supplier> items;
        private readonly ISupplierService _supplierService;
        private readonly IRegionManager _regionManager;

        public Supplier SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<Supplier> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        public DelegateCommand PaymentsCommand => new DelegateCommand(ShowPayments, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand PurchasesCommand => new DelegateCommand(ShowPurchases, () => SelectedItem != null).ObservesProperty(() => SelectedItem);

        

        #region Methods
        private void Add()
        {
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierEdit);
        }
        private void Update()
        {
            var parameters = new NavigationParameters { { "Supplier", SelectedItem } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierEdit, parameters);
        }
        private void Delete()
        {
            if (_supplierService.IsSupplierWithPurchases(SelectedItem))
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
            Items = new ObservableCollection<Model.Supplier>(_supplierService.SearchBy(SearchText));
        }
        private void ShowPurchases()
        {
            NavigationParameters parameters = new NavigationParameters { { "Supplier", SelectedItem } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierPurchasesMain, parameters);
        }
        private void ShowPayments()
        {
            NavigationParameters parameters = new NavigationParameters { { "Supplier", SelectedItem } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierPaymentMain, parameters);
        }
        #endregion
        public SuppliersMainViewModel(ISupplierService supplierService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _searchText = "";
            _regionManager = regionManager;
            _supplierService = supplierService;
            eventAggregator.GetEvent<SupplierAdded>().Subscribe(x=>Items.Add(x));
            eventAggregator.GetEvent<SupplierPaymentAdded>().Subscribe(RefreshSupplierPayments);
            eventAggregator.GetEvent<SupplierPaymentUpdated>().Subscribe(RefreshSupplierPayments);
            eventAggregator.GetEvent<SupplierPaymentDeleted>().Subscribe(RefreshSupplierPayments);
            Items = new ObservableCollection<Supplier>(_supplierService.GetAll());
        }

        private void RefreshSupplierPayments(SupplierPayment obj)
        {
            var supplier = Items.Single(x => x.ID == obj.SupplierID);
            Supplier newSupplier = _supplierService.Find(supplier.ID);
            DataUtils.Copy(supplier, newSupplier);
        }
    }
}
