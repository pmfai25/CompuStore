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
        private Dictionary<Tuple<int, decimal>, int> _dict;
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
            if (SelectedDetail.PurchaseItemID != 0)
            {
                var t = new Tuple<int, decimal>(SelectedDetail.ItemID, SelectedDetail.Price);
                if (_dict[t] - SelectedDetail.Quantity < 0)
                {
                    Messages.Error("لا يمكن حذف هذا الصنف " + SelectedDetail.Name + " لان تم بيعه بالكامل");
                    return;
                }
                else
                {
                    _dict[t] -= SelectedDetail.Quantity;
                    _deletedDetails.Add(new PurchaseItem() { ID = SelectedDetail.PurchaseItemID });
                }
            }
            Details.Remove(SelectedDetail);
            Purchase.Total = Details.Sum(i => i.Total);
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
            if (!CanSave())
                return;
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
                    lstInsert.Add(pi);
                else
                    lstUpdate.Add(pi);
            }
            if (lstInsert.Count > 0)
                _purchaseService.AddPurchaseItems(lstInsert);
            if (lstUpdate.Count > 0)
            {
                _purchaseService.UpdatePurchaseItems(lstUpdate);

            }
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

        private bool CanSave()
        {
            if (Purchase.Total <= 0)
            {
                Messages.Error("لا يمكن حفظ فاتورة بهذا الاجمالي " + Purchase.Total);
                return false;
            }
            if(Purchase.Paid <0)
            {
                Messages.Error("لا يمكن حفظ فاتورة بمبلغ مدفوع اقل من الصفر " + Purchase.Total);
                return false;
            }
            if (Purchase.Number <= 0)
            {
                Messages.Error("يجب اضافة رقم صحيح اكبر من الصفر للفاتورة ");
                return false;
            }
            var purchases = _supplierService.GetPurchases(Supplier);
            if(!_edit&&purchases.Any(x=>x.Number==Purchase.Number)|| _edit&&purchases.Any(x=>x.Number==Purchase.Number && x.ID!=Purchase.ID))
            {
                Messages.Error("رقم الفاتورة مكرر يجب ادخال رقم اخر ");
                return false;
            }
            return true;
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
            var p = new PurchaseDetails() { Name = item.Name, ItemID = item.ID,Quantity=1 };
            p.OnPriceUpdate += OnPriceUpdate;
            p.OnQuantityUpdate += OnQuantityUpdate;
            Details.Add(p);

        }

        private void AddItem()
        {
            _navigationContext.NavigationService.RequestNavigate(RegionNames.StoreEdit);
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
            _edit = Purchase.ID != 0;
            if(_edit)
            {
                _dict = new Dictionary<Tuple<int, decimal>, int>();
                var lst = _purchaseService.GetPatches(Purchase);
                foreach(var p in lst)
                    _dict.Add(new Tuple<int, decimal>(p.ItemID, p.Price), p.CurrentQuantity);
            }
            oldTotal = Purchase.Total;
            Details = (ObservableCollection<PurchaseDetails>)navigationContext.Parameters["Details"] ?? new ObservableCollection<PurchaseDetails>();
            foreach (var d in Details)
            {
                d.OnQuantityUpdate += OnQuantityUpdate;
                d.OnPriceUpdate += OnPriceUpdate;
            }
            _open = true;
        }

        private void OnQuantityUpdate(PurchaseDetails obj)
        {
            Purchase.Total = Details.Sum(x => x.Total);
        }

        private void OnPriceUpdate(PurchaseDetails obj)
        {
            //For merging items with same price and serial
            var lst = Details.Where(x => x.ItemID == obj.ItemID && x.Price == obj.Price).ToList();
            if(lst.Count>1)
            {
                var first = lst.FirstOrDefault(x => x.PurchaseItemID != 0) ?? lst[0];
                for(int i=1;i<lst.Count;i++)
                {
                    first.Quantity += lst[i].Quantity;
                    if (lst[i].PurchaseItemID != 0)
                        _deletedDetails.Add(new PurchaseItem { ID = lst[i].PurchaseItemID });
                    Details.Remove(lst[i]);
                }
            }
            Purchase.Total = Details.Sum(x => x.Total);

        }
        #endregion
        public SupplierPurchaseEditViewModel(IPurchaseService purchaseService,IItemService itemService,ISupplierService supplierService, IEventAggregator eventAggregator)
        {
            
            _supplierService = supplierService;
            _itemService = itemService;
            _purchaseService = purchaseService;
            _eventAggregator = eventAggregator;
            _deletedDetails = new List<PurchaseItem>();
            _eventAggregator.GetEvent<ItemAdded>().Subscribe(OnItemAdded);
        }
        private void OnItemAdded(Item obj)
        {
            var pd = new PurchaseDetails() { ItemID = obj.ID, Name = obj.Name };
            pd.OnQuantityUpdate += OnQuantityUpdate;
            pd.OnPriceUpdate += OnPriceUpdate;
            Details.Add(pd);            
            SelectedDetail = pd;
        }
    }
}
