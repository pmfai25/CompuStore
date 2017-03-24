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
    public class ClientOrdersReportViewModel : BindableBase,INavigationAware
    {
        #region Fields
        private IOrderService _orderService;
        private ObservableCollection<ClientOrders> _items;
        private DateTime _dateTo;
        private DateTime _dateFrom;
        private decimal _total;
        private decimal _paid;
        private decimal _remaining;
        private decimal _currentProfit;
        private decimal _finalProfit;
        private NavigationContext _navigationContext;
        #endregion
        #region Properties
        public ObservableCollection<ClientOrders> Items
        {
            get { return _items; }
            set
            {
                var t = new ObservableCollection<ClientOrders>(value.OrderBy(x => x.Name).ThenBy(y => y.Date));
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
        public decimal FinalProfit
        {
            get { return _finalProfit; }
            set { SetProperty(ref _finalProfit, value); }
        }
        public decimal CurrentProfit
        {
            get { return _currentProfit; }
            set { SetProperty(ref _currentProfit, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand BackCommand => new DelegateCommand(() => _navigationContext.NavigationService.Journal.GoBack());
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        #endregion
        #region Mehtods
        private void Search()
        {
            Items = new ObservableCollection<ClientOrders>(_orderService.GetOrders(DateFrom, DateTo));
            FixData();            
        }
        private void FixData()
        {
            Total = Paid = Remaining = CurrentProfit = FinalProfit = 0;
            if(Items.Count==0)
                return;
            foreach(var i in Items)
            {
                if (i.Date < _dateFrom)
                    _dateFrom = i.Date;
                if (i.Date > _dateTo)
                    _dateTo = i.Date;
                _total += i.Total;
                _paid += i.Paid;
                _currentProfit += i.CurrentProfit;
                _finalProfit += i.FinalProfit;                    
            }
            Remaining = Total - Paid;
            OnPropertyChanged("Total");
            OnPropertyChanged("Paid");
            OnPropertyChanged("FinalProfit");
            OnPropertyChanged("CurrentProfit");
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        #endregion
        public ClientOrdersReportViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            DateTo = DateFrom = DateTime.Today;
        }
    }
}
