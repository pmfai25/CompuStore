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
using System.Windows.Input;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPurchaseEditViewModel : BindableBase,INavigationAware, IRegionMemberLifetime
    {
        #region Fields
        private decimal oldTotal, newTotal;
        private List<PurchaseItem> _deletedDetails;
        private NavigationContext _navigationContext;
        private IEventAggregator _eventAggregator;
        private IPurchaseService _purchaseService;
        private IItemService _itemService;
        private ISupplierService _supplierService;
        private Purchase  _purchase;
        private Supplier _supplier;
        private ObservableCollection<PurchaseDetails> _details;
        private string _searchText;
        private PurchaseDetails _selectedDetail;
        private bool _open, _completed, _edit;
        private ICategoryService _categoryService;
        #endregion
        #region Properties
        public ObservableCollection<PurchaseDetails> Details
        {
            get { return _details; }
            set { SetProperty(ref _details, value); }
        }
        public Purchase Purchase
        {
            get { return _purchase; }
            set { SetProperty(ref _purchase, value); }
        }
        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
        }
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        public PurchaseDetails SelectedDetail
        {
            get { return _selectedDetail; }
            set { SetProperty(ref _selectedDetail, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete);
        public DelegateCommand AddItemCommand => new DelegateCommand(AddItem);
        public DelegateCommand<KeyEventArgs> SearchCommand => new DelegateCommand<KeyEventArgs>(Search);
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);
        #endregion
        #region Methods
        private void Delete()
        {
            if (SelectedDetail == null)
                return;
            if (SelectedDetail.PurchaseItemID != 0 && SelectedDetail.Available != SelectedDetail.Quantity)
            {
                Messages.Error("لا يمكن حذف هذا الصنف " + SelectedDetail.Name + " لانه تمت عليه عمليات بيع");
                return;
            }
            if (Messages.Delete(SelectedDetail.Name))
            {
                if (SelectedDetail.PurchaseItemID != 0)
                    _deletedDetails.Add(new PurchaseItem() { ID = SelectedDetail.PurchaseItemID });
                Details.Remove(SelectedDetail);
                Purchase.Total = Details.Sum(i => i.Total);
            }
        }
        private void Cancel()
        {
            if (_edit)
            {
                Purchase p2 = _purchaseService.FindPurchase(Purchase.ID);
                DataUtils.Copy(Purchase, p2);
            }
            _completed = true;
            NavigationParameters parameters = new NavigationParameters { { "Supplier", Supplier } };
            _navigationContext.NavigationService.RequestNavigate(RegionNames.SupplierPurchasesMain, parameters);
        }

        private void Save()
        {
            if (!Purchase.IsValid || Details.Any(x=>!x.IsValid))
            {
                Messages.Error("يوجد اخطاء في بعض البيانات");
                return;
            }
            SavePurchase();
            SaveDetails();                       
            _completed = true;
            if (_edit)
                _eventAggregator.GetEvent<PurchaseUpdated>().Publish(Purchase);
            else
                _eventAggregator.GetEvent<PurchaseAdded>().Publish(Purchase);
            NavigationParameters parameters = new NavigationParameters { { "Supplier", Supplier } };
            _navigationContext.NavigationService.RequestNavigate(RegionNames.SupplierPurchasesMain,parameters);
        }

        private void SaveDetails()
        {
            List<PurchaseItem> lstInsert = new List<PurchaseItem>();
            List<PurchaseItem> lstUpdate = new List<PurchaseItem>();
            foreach (var d in Details)
            {
                var pi = new PurchaseItem()
                {
                    ID = d.PurchaseItemID,
                    ItemID = d.ItemID,
                    PurchaseID = Purchase.ID,
                    Price = d.Price,
                    Quantity = d.Quantity
                };
                if (pi.ID == 0)
                {
                    pi.Available = pi.Quantity;
                    lstInsert.Add(pi);
                }
                else
                {
                    pi.Available = pi.Quantity - d.Sold;
                    lstUpdate.Add(pi);
                }
            }
            if (lstInsert.Count > 0)
                _purchaseService.AddPurchaseItems(lstInsert);
            if (lstUpdate.Count > 0)
                _purchaseService.UpdatePurchaseItems(lstUpdate);
            if (_deletedDetails.Count > 0)
                _purchaseService.DeletePurchaseItems(_deletedDetails);
        }

        private void SavePurchase()
        {
            newTotal = Purchase.Total;
            Purchase.Total = oldTotal;
            if (Purchase.ID == 0)
                _purchaseService.AddPurchase(Purchase);
            else
                _purchaseService.UpdatePurchase(Purchase);
            Purchase.Total = newTotal;
        }
        private void AddItem()
        {
            ObservableCollection<Category> categories = new ObservableCollection<Category>(_categoryService.GetAll());
            if(categories.Count==0)
            {
                Messages.Error("يجب اضافة اقسام من شاشة الاصناف اولا قبل اضافة صنف جديد");
                return;
            }
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Categories", categories);
            _navigationContext.NavigationService.RequestNavigate(RegionNames.StoreEdit);
        }
        private void Search(KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            long s;
            if (!long.TryParse(SearchText, out s))
                return;
            var item = _itemService.SearchBySerial(s);
            if (item == null)
            {
                Messages.Notification("لايوجد صنف بهذا الباركود");
                return;
            }
            OnItemAdded(item);
        }
        private void OnItemAdded(Item obj)
        {
            var pd = new PurchaseDetails() { ItemID = obj.ID, Name = obj.Name, Quantity=1 };
            pd.OnUpdateValues +=()=> Purchase.Total = Details.Sum(x => x.Total);
            Details.Add(pd);
            SelectedDetail = pd;
        }     
        #endregion
        #region Interface
        public bool KeepAlive
        {
            get
            {
                return !_completed;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (_open)
                return;
            _navigationContext = navigationContext;
            Supplier = (Supplier)navigationContext.Parameters["Supplier"];
            Purchase =(Purchase)navigationContext.Parameters["Purchase"]??new Purchase() { Date = DateTime.Today, SupplierID = _supplier.ID };
            Purchase.SupplierPurchases = _supplierService.GetPurchases(Supplier);
            _edit = Purchase.ID != 0;
            if (!_edit)
                if (Purchase.SupplierPurchases.Count > 0)
                    Purchase.Number = Purchase.SupplierPurchases.Max(x=>x.Number) + 1;
                else
                    Purchase.Number = 1;
            oldTotal = Purchase.Total;
            Details = (ObservableCollection<PurchaseDetails>)navigationContext.Parameters["Details"] ?? new ObservableCollection<PurchaseDetails>();
            foreach (var d in Details)
                d.OnUpdateValues +=()=> Purchase.Total = Details.Sum(x => x.Total);
            _open = true;
        }
        #endregion
        public SupplierPurchaseEditViewModel(IPurchaseService purchaseService,IItemService itemService,ICategoryService categoryService,ISupplierService supplierService, IEventAggregator eventAggregator)
        {
            _categoryService = categoryService;
            _supplierService = supplierService;
            _itemService = itemService;
            _purchaseService = purchaseService;
            _eventAggregator = eventAggregator;
            _deletedDetails = new List<PurchaseItem>();
            _eventAggregator.GetEvent<ItemAdded>().Subscribe(OnItemAdded);
        }     
    }
}
