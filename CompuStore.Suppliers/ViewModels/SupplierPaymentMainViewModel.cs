﻿using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Events;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPaymentMainViewModel : BindableBase,INavigationAware,IRegionMemberLifetime
    {
        #region Fields
        private Supplier _supplier;
        private decimal _total;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private SupplierPayment _selectedItem;
        private ObservableCollection<SupplierPayment> _items;
        private ISupplierPaymentService _supplierPaymentService;
        private NavigationContext _navigationContext;
        private IEventAggregator _eventAggregator;
        #endregion
        #region Properties
        public decimal Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
        }
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value);}
        }
        public SupplierPayment SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<SupplierPayment> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }
        #endregion
        #region Commands
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand BackCommand => new DelegateCommand(Back);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);
        #endregion
        #region Methods
        private void Refresh()
        {
            Items = new ObservableCollection<SupplierPayment>(_supplierPaymentService.GetAll(Supplier));
            if (Items.Count > 0)
            {
                DateFrom = Items.Min(x => x.Date).Date;
                DateTo = Items.Max(x => x.Date).Date;
            }
            else
                DateFrom = DateTo = DateTime.Today;
            Total = Items.Sum(s => s.Money);
        }
        private void Back()
        {
            if (_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }
        private void Add()
        {
            var parameters = new NavigationParameters { { "Supplier", _supplier } };
            _navigationContext.NavigationService.RequestNavigate(RegionNames.SupplierPaymentEdit, parameters);
        }
        private void Update()
        {
            var parameters = new NavigationParameters { { "SupplierPayment", SelectedItem } };
            _navigationContext.NavigationService.RequestNavigate( RegionNames.SupplierPaymentEdit, parameters);
        }
        private void Delete()
        {
            if (!Messages.Delete("فاتورة ايصال نقدية رقم " + SelectedItem.Number.ToString())) return;
            if (!_supplierPaymentService.Delete(SelectedItem))
            {
                Messages.ErrorDataNotSaved();
                return;
            }
            _eventAggregator.GetEvent<SupplierPaymentDeleted>().Publish(SelectedItem);
            Items.Remove(SelectedItem);
            Total = Items.Sum(x => x.Money);
        }
        private void Search()
        {
            Items = new ObservableCollection<SupplierPayment>(_supplierPaymentService.SearchByInterval(Supplier, DateFrom, DateTo));
            Total = Items.Sum(x => x.Money);
        }
        private void OnSupplierPaymentUpdated(SupplierPayment obj)
        {
            Search();
        }

        private void OnSupplierPaymentAdded(SupplierPayment obj)
        {
            Items.Add(obj);
            Search();
        }
        #endregion
        #region Interfaces
        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            Supplier = (Supplier)(navigationContext.Parameters["Supplier"]);
            RefreshCommand.Execute();
            _navigationContext = navigationContext;
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion
        public SupplierPaymentMainViewModel(ISupplierPaymentService supplierPaymentService, IEventAggregator eventAggregator)
        {
            _supplierPaymentService = supplierPaymentService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SupplierPaymentAdded>().Subscribe(OnSupplierPaymentAdded);
            _eventAggregator.GetEvent<SupplierPaymentUpdated>().Subscribe(OnSupplierPaymentUpdated);
        }
    }
}
