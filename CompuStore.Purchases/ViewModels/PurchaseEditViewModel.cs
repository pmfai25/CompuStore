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

namespace CompuStore.Purchases.ViewModels
{
    public class PurchaseEditViewModel : BindableBase,INavigationAware,IRegionMemberLifetime
    {
        private List<PurchaseItem> lstDeleted;
        private bool completed;
        private bool _edit;
        private IItemService itemService;
        private SupplierPurchases purchase;
        private IPurchaseService purchaseService;
        private ISupplierService supplierService;
        private NavigationContext navigationContext;
        private ObservableCollection<PurchaseDetails> details;
        private long searchText;
        private ObservableCollection<Item> items;
        private PurchaseDetails selectedDetail;
        private ObservableCollection<Supplier> suppliers;
        private IEventAggregator eventAggregator;
        private Supplier selectedSupplier;
        public PurchaseDetails SelectedDetail
        {
            get { return selectedDetail; }
            set { SetProperty(ref selectedDetail, value); }
        }
        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set { SetProperty(ref selectedSupplier, value); }
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get { return suppliers; }
            set { SetProperty(ref suppliers, value); }
        }
        public long SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); Search(); }
        }
        public ObservableCollection<Item> Items
        {
            get { return items; ; }
            set { SetProperty(ref items, value); }
        }
        public ObservableCollection<PurchaseDetails> Details
        {
            get { return details; }
            set { SetProperty(ref details, value); }
        }
        public SupplierPurchases Purchase
        {
            get { return purchase; }
            set { SetProperty(ref purchase, value); }
        }
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete);
        public DelegateCommand AddSupplierCommand => new DelegateCommand(AddSupplier);
        public DelegateCommand AddItemCommand => new DelegateCommand(AddItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);

        public bool KeepAlive
        {
            get
            {
                return !completed;
            }
        }
        private void Delete()
        {
            if(SelectedDetail.PurchaseItemID!=0)
                lstDeleted.Add(new PurchaseItem() { ID = SelectedDetail.PurchaseItemID });
            Details.Remove(SelectedDetail);
            Purchase.Total = Details.Sum(i => i.Total);
        }
        private void Cancel()
        {
            if(_edit)
            {
                SupplierPurchases p2 = purchaseService.FindPurchaseDetails(Purchase.PurchaseID);
                DataUtils.Copy(Purchase, p2);
            }
            completed = true;
            navigationContext.NavigationService.Journal.GoBack();
        }

        private void Save()
        {
            if(Purchase.Total<=0)
            {
                Messages.Error("لا يمكن حفظ فاتورة بهذا الاجمالي " + Purchase.Total);
                return;
            }
            Purchase p = new Purchase();
            p.ID = Purchase.PurchaseID;
            p.SupplierID = SelectedSupplier.ID;
            p.Date = Purchase.Date;
            p.Number = Purchase.Number;
            p.Total = Purchase.Total;
            p.Paid = Purchase.Paid;
            if(!purchaseService.AddPurchase(p))
            {
                Messages.ErrorDataNotSaved();
                return;
            }
            List<PurchaseItem> lstInsert = new List<PurchaseItem>();
            List<PurchaseItem> lstUpdate = new List<PurchaseItem>();
            foreach (var d in Details)
            {
                var pi = new PurchaseItem()
                {
                    ID = d.PurchaseItemID,
                    ItemID = d.ItemID,
                    PurchaseID = p.ID,
                    Price = d.Price,
                    Quantity = d.Quantity
                };
                if (pi.ID == 0)
                    lstInsert.Add(pi);
                else
                    lstUpdate.Add(pi);
            }
            if (lstInsert.Count > 0)
                purchaseService.AddPurchaseItems(lstInsert);
            if (lstUpdate.Count > 0)
                purchaseService.UpdatePurchaseItems(lstUpdate);
            if (lstDeleted.Count > 0)
                purchaseService.DeletePurchaseItems(lstDeleted);
            completed = true;            
        }


        private void Search()
        {
            var item = Items.SingleOrDefault(x => x.Serial == SearchText);
            if (item == null)
                return;
            var p = new PurchaseDetails() { Name = item.Name, ItemID = item.ID };
            p.UpdateValues += () => Purchase.Total = Details.Sum(x => x.Total);
            Details.Add(p);
           
        }        
        private void AddItem()
        {
            navigationContext.NavigationService.RequestNavigate(RegionNames.StoreEdit);
        }

        private void AddSupplier()
        {
            navigationContext.NavigationService.RequestNavigate(RegionNames.SupplierEdit);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (_edit)
                return;
            this.navigationContext = navigationContext;
            Purchase = (SupplierPurchases)navigationContext.Parameters["SupplierPurchase"] ?? new SupplierPurchases() { Date = DateTime.Today };
            _edit = Purchase.PurchaseID != 0;
            if (_edit)
            {
                var lst = purchaseService.GetPurchaseDetails(Purchase);
                foreach (var p in lst)
                {
                    p.UpdateValues += () => Purchase.Total = Details.Sum(x => x.Total);
                    Details.Add(p);
                }
                SelectedSupplier = Suppliers.Single(x => x.ID == Purchase.SupplierID);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public PurchaseEditViewModel(ISupplierService supplierService,IPurchaseService purchaseService,IItemService itemService,IEventAggregator eventAggregator)
        {
            lstDeleted = new List<PurchaseItem>();
            this.itemService = itemService;
            this.supplierService = supplierService;
            this.purchaseService = purchaseService;
            this.eventAggregator = eventAggregator;
            Suppliers = new ObservableCollection<Supplier>(supplierService.GetAll(true));
            SelectedSupplier = Suppliers.FirstOrDefault();
            Items = new ObservableCollection<Item>(itemService.GetAll(true));
            Details = new ObservableCollection<PurchaseDetails>();
            eventAggregator.GetEvent<SupplierAdded>().Subscribe(OnSupplierAdded);
            eventAggregator.GetEvent<ItemAdded>().Subscribe(OnItemAdded);
        }

        private void OnItemAdded(Item obj)
        {
            Items.Add(obj);
            Details.Add(new PurchaseDetails() { ItemID = obj.ID, Name = obj.Name });
        }

        private void OnSupplierAdded(Supplier obj)
        {
            Suppliers.Add(obj);
            SelectedSupplier = obj;
        }
    }
}
