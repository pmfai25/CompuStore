using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Model.Views;
using Service;
using Prism.Regions;

namespace CompuStore.Reports.ViewModels
{
    public class IncomeOutcomeReportViewModel : BindableBase,INavigationAware
    {
        #region Field
        private NavigationContext _navigationContext;
        private IReportService _reportService;
        private DateTime _dateTo;
        private DateTime _dateFrom;
        private List<ViewIncome> _income;
        private List<ViewOutcome> _outcome;
        private decimal _totalIncome;
        private decimal _totalOutcome;
        private decimal _previous;
        private decimal _current;
        private decimal _remaining;

        #endregion
        #region Properties
        public decimal Remaining
        {
            get { return _remaining; }
            set { SetProperty(ref _remaining, value); }
        }
        public decimal Current
        {
            get { return _current; }
            set { SetProperty(ref _current, value); }
        }
        public decimal  Previous
        {
            get { return _previous; }
            set { SetProperty(ref _previous, value); }
        }
        public decimal TotalOutcome
        {
            get { return _totalOutcome; }
            set { SetProperty(ref _totalOutcome, value); }
        }
        public decimal TotalIncome
        {
            get { return _totalIncome; }
            set { SetProperty(ref _totalIncome, value); }
        }
        public List<ViewOutcome> Outcome
        {
            get { return _outcome; }
            set { SetProperty(ref _outcome, value); }
        }
        public List<ViewIncome> Income
        {
            get { return _income; }
            set { SetProperty(ref _income, value); }
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
        #endregion
        #region Commands
        public DelegateCommand BackCommand => new DelegateCommand(() => _navigationContext.NavigationService.Journal.GoBack());
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        #endregion
        public IncomeOutcomeReportViewModel(IReportService reportService)
        {
            _reportService = reportService;
            DateTo = DateFrom = DateTime.Today;
        }
        #region Methods
        private void Search()
        {
            Income = _reportService.GetIncome(DateFrom, DateTo);
            Outcome = _reportService.GetOutcome(DateFrom, DateTo);
            Previous = _reportService.GetCurrentMoney(DateFrom.AddDays(-1));
            TotalIncome = Income.Sum(x => x.Money);
            TotalOutcome = Outcome.Sum(x => x.Money);
            Remaining = TotalIncome - TotalOutcome;
            Current = Previous + Remaining;
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
    }
}
