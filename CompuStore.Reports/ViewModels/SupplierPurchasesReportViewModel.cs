using Model.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Reports.ViewModels
{
    public class SupplierPurchasesReportViewModel : BindableBase,INavigationAware
    {
        #region Fields
        private IPurchaseService _purchaseService;
        private ObservableCollection<SupplierPurchases> _items;
        private DateTime _dateTo;
        private DateTime _dateFrom;
        private decimal _total;
        private decimal _paid;
        private decimal _remaining;       
        #endregion
        #region Properties
        public ObservableCollection<SupplierPurchases> Items
        {
            get { return _items; }
            set
            {
                var t = new ObservableCollection<SupplierPurchases>(value.OrderBy(x => x.Name).ThenBy(y => y.Date));
                SetProperty(ref _items, t);
            }
        }
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }
        public decimal Remaining
        {
            get { return _remaining; }
            set { SetProperty(ref _remaining, value); }
        }
        public decimal Paid
        {
            get { return _paid; }
            set { SetProperty(ref _paid, value); }
        }
        public decimal Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);
        #endregion
        #region Mehtods
        private void Refresh()
        {
            Items = new ObservableCollection<SupplierPurchases>(_purchaseService.GetPurchases());
            if (Items.Count > 0)
            {
                DateFrom = Items.Min(x => x.Date).Date;
                DateTo = Items.Max(x => x.Date).Date;
                Total = Items.Sum(x => x.Total);
                Paid = Items.Sum(x => x.Paid);
                Remaining = Total - Paid;
            }
            else
                DateFrom = DateTo = DateTime.Today;
        }
        private void Search()
        {
            Items = new ObservableCollection<SupplierPurchases>(_purchaseService.GetPurchases(DateFrom,DateTo));
            if (Items.Count == 0)
                return;
            Total = Items.Sum(x => x.Total);
            Paid = Items.Sum(x => x.Paid);
            Remaining = Total - Paid;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        #endregion
        public SupplierPurchasesReportViewModel(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
            Refresh();
        }
    }
}
