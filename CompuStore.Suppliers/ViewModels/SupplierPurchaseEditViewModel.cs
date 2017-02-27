using Model;
using Model.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPurchaseEditViewModel : BindableBase,INavigationAware, IRegionMemberLifetime
    {
        private NavigationContext _navigationContext;
        private IEventAggregator _eventAggregator;
        private IPurchaseService _purchaseService;
        private Purchase  _purchase;
        private Supplier _supplier;
        private PurchaseDetails _details;
        public PurchaseDetails Details
        {
            get { return _details; }
            set { SetProperty(ref _details, value); }
        }
        public Purchase Purchase
        {
            get { return _purchase; }
            set { SetProperty(ref _purchase, value); }
        }
        public SupplierPurchaseEditViewModel(IPurchaseService purchaseService, IEventAggregator eventAggregator)
        {
            _purchaseService = purchaseService;
            _eventAggregator = eventAggregator;
        }

        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            _supplier = (Supplier)navigationContext.Parameters["Supplier"];
            if (_supplier != null)
                Purchase = new Purchase() { Date = DateTime.Today, SupplierID = _supplier.ID };
            else
                Purchase = (Purchase)navigationContext.Parameters["Purchase"];
        }
    }
}
