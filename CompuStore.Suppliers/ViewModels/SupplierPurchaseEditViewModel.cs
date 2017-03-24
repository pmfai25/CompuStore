using CompuStore.Infrastructure;
using CompuStore.Store.Confirmations;
using CompuStore.Suppliers.Confirmations;
using Model;
using Model.Views;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPurchaseEditViewModel : BindableBase, IInteractionRequestAware
    {
        #region Fields
        private SupplierPurchaseConfirmation _confirmation;
        private decimal _oldTotal, _newTotal;
        private string _searchText;
        private List<PurchaseItem> _deletedDetails;
        private Purchase _purchase;
        private ObservableCollection<PurchaseDetails> _details;
        private PurchaseDetails _selectedDetail;
        private IPurchaseService _purchaseService;
        private IItemService _itemService;
        #endregion
        #region Properties
        public InteractionRequest<ItemConfirmation> NewItemRequest { get; set; }
        public INotification Notification
        {
            get
            {
                return _confirmation;
            }

            set
            {
                _confirmation = (SupplierPurchaseConfirmation)value;
                Purchase = _confirmation.SupplierPurchase;
                Details = _confirmation.Details;
                _oldTotal = Purchase.Total;
                foreach (var d in Details)
                    d.OnUpdateValues += () => Purchase.Total = Details.Sum(x => x.Total);
                OnPropertyChanged(() => Notification);
            }
        }
        public Action FinishInteraction
        {
            set; get;
        }
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
        public DelegateCommand DeleteItemCommand => new DelegateCommand(Delete);
        public DelegateCommand AddItemCommand => new DelegateCommand(AddItem);
        public DelegateCommand<KeyEventArgs> SearchCommand => new DelegateCommand<KeyEventArgs>(Search);
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public DelegateCommand CancelCommand => new DelegateCommand(()=>FinishInteraction());
        public DelegateCommand SetPaidCommand => new DelegateCommand(() => Purchase.Paid = Purchase.Total);
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
        private void Save()
        {
            if (!Purchase.IsValid || Details.Any(x=>!x.IsValid))
            {
                Messages.ErrorValidation();
                return;
            }
            SavePurchase();
            SaveDetails();
            _confirmation.Confirmed = true;
            FinishInteraction();        
            
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
            _newTotal = Purchase.Total;
            Purchase.Total = _oldTotal;
            if (Purchase.ID == 0)
                _purchaseService.AddPurchase(Purchase);
            else
                _purchaseService.UpdatePurchase(Purchase);
            Purchase.Total = _newTotal;
        }
        private void AddItem()
        {
            ItemConfirmation y = new ItemConfirmation();
            y.Item = new Item();
            NewItemRequest.Raise(y, x =>
            {
                if (x.Confirmed)
                {
                    _itemService.Add(x.Item);
                    OnItemAdded(x.Item);
                }
            });
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
                Messages.Notification(" لايوجد صنف بهذا الباركود " + SearchText);
                SearchText = "";
                return;
            }
            OnItemAdded(item);
            SearchText = "";
        }
        private void OnItemAdded(Item obj)
        {
            var pd = new PurchaseDetails() { ItemID = obj.ID, Name = obj.Name, Quantity=1 };
            pd.OnUpdateValues +=()=> Purchase.Total = Details.Sum(x => x.Total);
            Details.Add(pd);
            SelectedDetail = pd;
        }     
        #endregion       
        public SupplierPurchaseEditViewModel(IPurchaseService purchaseService,IItemService itemService)
        {
            _purchaseService = purchaseService;
            _itemService = itemService;
            _deletedDetails = new List<PurchaseItem>();
            NewItemRequest = new InteractionRequest<ItemConfirmation>();
        }
    }
}
