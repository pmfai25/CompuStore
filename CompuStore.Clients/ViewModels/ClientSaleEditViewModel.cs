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

namespace CompuStore.Clients.ViewModels
{
    public class ClientSaleEditViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        #region Fields
        private decimal oldTotal, newTotal;
        private List<OrderItem> _deletedDetails;
        private NavigationContext _navigationContext;
        private IEventAggregator _eventAggregator;
        private IOrderService _orderService;
        private IItemService _itemService;
        private IPurchaseService _purchaseService;
        private readonly IClientService _clientService;
        private Orders _order;
        private Client _client;
        private ObservableCollection<OrderDetails> _details;
        private string _searchText;
        private OrderDetails _selectedDetail;
        private bool _open, _completed, _edit;
        #endregion
        #region Properties
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
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
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
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete);
        public DelegateCommand<KeyEventArgs> SearchCommand => new DelegateCommand<KeyEventArgs>(Search);
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);
        #endregion
        #region Methods
        private void Delete()
        {
            if (!Messages.Delete(SelectedDetail.Name))
                return;
            if (SelectedDetail.OrderItemID != 0)
                _deletedDetails.Add(new OrderItem() { ID = SelectedDetail.OrderItemID });
            Details.Remove(SelectedDetail);
            Order.Total = Details.Sum(i => i.Total);
        }
        private void Cancel()
        {
            if (_edit)
            {
                Orders o2 = _orderService.FindOrder(Order.ID);
                DataUtils.Copy(Order, o2);
            }
            _completed = true;
            NavigationParameters parameters = new NavigationParameters { { "Client", Client } };
            _navigationContext.NavigationService.RequestNavigate(RegionNames.ClientSalesMain, parameters);
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
            _completed = true;
            if (_edit)
                _eventAggregator.GetEvent<OrderUpdated>().Publish(Order);
            else
                _eventAggregator.GetEvent<OrderAdded>().Publish(Order);
            NavigationParameters parameters = new NavigationParameters { { "Client", Client } };
            _navigationContext.NavigationService.RequestNavigate(RegionNames.ClientSalesMain, parameters);
        }

        private void SaveOrder()
        {
            newTotal = Order.Total;
            Order.Total = oldTotal;
            if (Order.ID == 0)
                _orderService.AddOrder(Order);
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
                    Messages.Notification("لايوجد صنف بهذا الباركود");
                    return;
                }
                if(item.Quantity==0)
                {
                    Messages.Notification("لا توجد كمية متاحة من " + item.Name);
                    return;
                }
                List<PurchaseItem> purchaseItems = _purchaseService.GetPurchaseItemsWithStock(item);
                var ids = Details.Where(x => x.Serial == s).Select(y => y.PurchaseItemID);
                var selectedPurchaseItem = purchaseItems.Where(x => !ids.Contains(x.ID)).FirstOrDefault();
                if(selectedPurchaseItem==null)
                {
                    Messages.Notification("لا توجد كمية متاحة من " + item.Name);
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
            Client = (Client)navigationContext.Parameters["Client"];
            Order = (Orders)navigationContext.Parameters["Order"] ?? new Orders() { Date = DateTime.Today, ClientID = Client.ID };
            Order.ClientOrders = _clientService.GetOrders(Client);
            _edit = Order.ID != 0;
            if (!_edit)
                if (Order.ClientOrders.Count > 0)
                    Order.Number = Order.ClientOrders.Max(x => x.Number) + 1;
                else
                    Order.Number = 1;
            oldTotal = Order.Total;
            Details = (ObservableCollection<OrderDetails>)navigationContext.Parameters["Details"] ?? new ObservableCollection<OrderDetails>();
            foreach (var d in Details)
                d.UpdateValues += ()=> Order.Total = Details.Sum(x => x.Total);
            _open = true;
        }
        #endregion
        public ClientSaleEditViewModel(IOrderService orderService,IPurchaseService purchaseService, IItemService itemService, IClientService clientService, IEventAggregator eventAggregator)
        {
            _purchaseService = purchaseService;
            _clientService = clientService;
            _itemService = itemService;
            _orderService = orderService;
            _eventAggregator = eventAggregator;
            _deletedDetails = new List<OrderItem>();
        }
    }
}
