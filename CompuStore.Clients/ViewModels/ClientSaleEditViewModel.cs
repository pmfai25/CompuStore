using CompuStore.Clients.Confirmations;
using CompuStore.Infrastructure;
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CompuStore.Clients.ViewModels
{
    public class ClientSaleEditViewModel : BindableBase, IInteractionRequestAware
    {
        #region Fields
        private ClientSaleConfirmation _confirmation;
        private decimal oldTotal, newTotal;
        private string _searchText;
        private List<OrderItem> _deletedDetails;  
        private Orders _order;
        private ObservableCollection<OrderDetails> _details;
        private OrderDetails _selectedDetail;
        private IOrderService _orderService;
        private IItemService _itemService;
        private IPurchaseService _purchaseService;
        private Account _account;
        #endregion
        #region Properties
        public INotification Notification
        {
            get
            {
                return _confirmation;
            }

            set
            {
                _confirmation = (ClientSaleConfirmation)value;
                Order = _confirmation.ClientOrder;
                Details = _confirmation.Details;
                oldTotal = Order.Total;
                foreach (var d in Details)
                    d.UpdateValues += () => Order.Total = Details.Sum(x => x.Total);
                OnPropertyChanged(() => Notification);
            }
        }

        public Action FinishInteraction
        {
            get;set;
        }
        public ObservableCollection<OrderDetails> Details
        {
            get { return _details; }
            set { SetProperty(ref _details, value); }
        }
        public Orders Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        public OrderDetails SelectedDetail
        {
            get { return _selectedDetail; }
            set { SetProperty(ref _selectedDetail, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand DeleteItemCommand => new DelegateCommand(Delete);
        public DelegateCommand<KeyEventArgs> SearchCommand => new DelegateCommand<KeyEventArgs>(Search);
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public DelegateCommand CancelCommand => new DelegateCommand(() => FinishInteraction());
        public DelegateCommand SetPaidCommand => new DelegateCommand(() => Order.Paid = Order.Total);
        #endregion
        #region Methods
        private void Delete()
        {
            if (SelectedDetail == null)
                return;
            if (!Messages.Delete(SelectedDetail.Name))
                return;
            if (SelectedDetail.OrderItemID != 0)
                _deletedDetails.Add(new OrderItem() { ID = SelectedDetail.OrderItemID });
            Details.Remove(SelectedDetail);
            Order.Total = Details.Sum(i => i.Total);
        }
        private void Save()
        {
            if (!Order.IsValid || Details.Any(x => !x.IsValid))
            {
                Messages.Error("يوجد اخطاء في بعض البيانات");
                return;
            }
            SaveOrder();
            SaveDetails();
            _confirmation.Confirmed = true;
            FinishInteraction();
        }

        private void SaveOrder()
        {
            newTotal = Order.Total;
            Order.Total = oldTotal;
            if (Order.ID == 0)
            {
                Order.AccountID = _account.ID;
                _orderService.AddOrder(Order);
            }
            else
                _orderService.UpdateOrder(Order);
            Order.Total = newTotal;
        }

        private void SaveDetails()
        {
            List<OrderItem> lstInsert = new List<OrderItem>();
            List<OrderItem> lstUpdate = new List<OrderItem>();
            foreach (var d in Details)
            {
                var oi = new OrderItem()
                {
                    ID = d.OrderItemID,
                    PurchaseItemID=d.PurchaseItemID,
                    Retail=d.Retail,
                    OrderID=Order.ID,
                    Price = d.Price,
                    Quantity = d.Quantity,
                    Discount=d.Discount
                };
                if (oi.ID == 0)
                    lstInsert.Add(oi);
                else
                    lstUpdate.Add(oi);                
            }
            if (lstInsert.Count > 0)
                _orderService.AddOrderItems(lstInsert);
            if (lstUpdate.Count > 0)
                _orderService.UpdateOrderItems(lstUpdate);
            if (_deletedDetails.Count > 0)
                _orderService.DeleteOrderItems(_deletedDetails);
        }
        private void Search(KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            long s;
            if (!long.TryParse(SearchText, out s))
                return;
            var oldDetail = Details.FirstOrDefault(x => x.Serial == s && x.Available+x.OldQuantity>x.Quantity);
            if (oldDetail != null)
                oldDetail.Quantity++;
            else
            {
                var item = _itemService.SearchBySerial(s);
                if (item == null)
                {
                    Messages.Notification(" لايوجد صنف بهذا الباركود "+ SearchText);
                    SearchText = "";
                    return;
                }
                
                if(item.Quantity==0)
                {
                    Messages.Notification("لا توجد كمية متاحة من " + item.Name);
                    SearchText = "";
                    return;
                }
                List<PurchaseItem> purchaseItems = _purchaseService.GetPurchaseItemsWithStock(item);
                var ids = Details.Where(x => x.Serial == s).Select(y => y.PurchaseItemID);
                var selectedPurchaseItem = purchaseItems.Where(x => !ids.Contains(x.ID)).FirstOrDefault();
                if(selectedPurchaseItem==null)
                {
                    Messages.Notification("لا توجد كمية متاحة من " + item.Name);
                    SearchText = "";
                    return;
                }
                OrderDetails od = new OrderDetails();
                Details.Add(od);
                od.UpdateValues += () => Order.Total = Details.Sum(x => x.Total);
                od.PurchaseItemID = selectedPurchaseItem.ID;
                od.Available = selectedPurchaseItem.Available;
                od.Retail = selectedPurchaseItem.Price;
                od.Name = item.Name;
                od.ItemID = item.ID;
                od.Serial = item.Serial;
                od.Price = item.Price;
                od.Quantity = 1;
            }
            SearchText = "";
        }
        #endregion
        public ClientSaleEditViewModel(IOrderService orderService,IPurchaseService purchaseService, IItemService itemService, Account account)
        {
            _account = account;
            _purchaseService = purchaseService;
            _itemService = itemService;
            _orderService = orderService;
            _deletedDetails = new List<OrderItem>();
        }
    }
}
