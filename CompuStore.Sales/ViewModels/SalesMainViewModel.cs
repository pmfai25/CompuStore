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
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Sales.ViewModels
{
    public class SalesMainViewModel : BindableBase
    {
        private IOrderService orderService;
        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;
        private Order selectedItem;
        private ObservableCollection<Order> items;
        private decimal total;
        private DateTime dateFrom;
        private DateTime dateTo;
        private ObservableCollection<OrderDetails> details;
        public DateTime DateTo
        {
            get { return dateTo; }
            set { SetProperty(ref dateTo, value); }
        }
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { SetProperty(ref dateFrom, value); }
        }
        public decimal Total
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }
        public Order SelectedItem
        {
            get { return selectedItem; }
            set
            {
                SetProperty(ref selectedItem, value);
                if (SelectedItem != null)
                    Details = new ObservableCollection<OrderDetails>(orderService.GetOrderDetails(SelectedItem));
            }
        }
        public ObservableCollection<Order> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }
        public ObservableCollection<OrderDetails> Details
        {
            get { return details; }
            set { SetProperty(ref details, value); }
        }
        public DelegateCommand AddCommand => new DelegateCommand(Add);

        private void Add()
        {
            regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SalesEdit);
        }
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);

        private void Update()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("ClientOrder", SelectedItem);
            regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SalesEdit);
        }
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);

        private void Delete()
        {
            if (Messages.Delete("فاتورة شراء للعميل " + selectedItem.Name))
            {
                orderService.DeleteOrder(new Order() { ID = SelectedItem.OrderID });
                Items.Remove(SelectedItem);
                Total = Items.Sum(i => i.Total);
            }
        }
        public DelegateCommand SearchCommand => new DelegateCommand(Search);

        private void Search()
        {
            Items = new ObservableCollection<ClientOrders>(orderService.GetClientOrders(DateFrom, DateTo));
            SelectedItem = Items.FirstOrDefault();
        }
        public SalesMainViewModel(IOrderService orderService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.orderService = orderService;
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            eventAggregator.GetEvent<OrderAdded>().Subscribe(OnOrderAdded);
            eventAggregator.GetEvent<OrderUpdated>().Subscribe(OnOrderUpdated);
            DateTo = DateTime.Today;
            DateFrom = DateTime.Today;
            Search();
        }

        private void OnOrderUpdated(Order obj)
        {
            Total = Items.Sum(i => i.Total);
        }

        private void OnOrderAdded(Order obj)
        {
            Items.Add(obj);
            Total = Items.Sum(i => i.Total);
        }
    }
}
