using CompuStore.Infrastructure;
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
    public class PurchasesMainViewModel : BindableBase
    {
        private IPurchaseService purchaseService;
        private IRegionManager regionManager;
        private IEventAggregator eventAggregator;
        private SupplierPurchases selectedItem;
        private ObservableCollection<SupplierPurchases> items;
        private decimal total;
        private DateTime dateFrom;
        private DateTime dateTo;
        private ObservableCollection<PurchaseDetails> details;
        
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
        public SupplierPurchases SelectedItem
        {
            get { return selectedItem; }
            set
            {
                SetProperty(ref selectedItem, value);
                if (SelectedItem != null)
                    Details = new ObservableCollection<PurchaseDetails>(purchaseService.GetPurchaseDetails(SelectedItem));
            }
        }       
        public ObservableCollection<SupplierPurchases> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }
        public ObservableCollection<PurchaseDetails> Details
        {
            get { return details; }
            set { SetProperty(ref details, value); }
        }
        #region Commands
        public DelegateCommand AddCommand => new DelegateCommand(Add);

        private void Add()
        {
            regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.PurchaseEdit);
        }

        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);

        private void Update()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("SupplierPurchase", SelectedItem);
            regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.PurchaseEdit,parameters);
        }

        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);

        private void Delete()
        {
            if (Messages.Delete("فاتورة شراء للشركة " + selectedItem.Name))
            {
                purchaseService.DeletePurchase(new Model.Purchase() { ID = SelectedItem.PurchaseID });
                Items.Remove(SelectedItem);
                Total = Items.Sum(i => i.Total);
            }
        }

        public DelegateCommand SearchCommand => new DelegateCommand(Search);

        private void Search()
        {
            Items = new ObservableCollection<SupplierPurchases>(purchaseService.GetSupplierPurchases(DateFrom, DateTo));
            SelectedItem = Items.FirstOrDefault();
        }
        #endregion
        public PurchasesMainViewModel(IPurchaseService purchaseService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            this.purchaseService = purchaseService;
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            eventAggregator.GetEvent<PurchaseAdded>().Subscribe(OnPurchaseAdded);
            eventAggregator.GetEvent<PurchaseUpdated>().Subscribe(OnPurchaseUpdated);
            DateTo = DateTime.Today;
            DateFrom = DateTime.Today;
            Search();
        }

        private void OnPurchaseUpdated(SupplierPurchases obj)
        {
            Total = Items.Sum(i => i.Total);
        }

        private void OnPurchaseAdded(SupplierPurchases obj)
        {
            Items.Add(obj);
            Total = Items.Sum(i => i.Total);
        }
    }
}
