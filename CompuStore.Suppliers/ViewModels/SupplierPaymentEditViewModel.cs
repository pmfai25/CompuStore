using CompuStore.Infrastructure;
using CompuStore.Suppliers.Model;
using CompuStore.Suppliers.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPaymentEditViewModel : BindableBase, INavigationAware
    {
        #region Fields
        private bool _edit;
        private SupplierPayment _supplierPayment;
        private NavigationContext _navigationContext;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private ISupplierPaymentService _supplierPaymentService;
        #endregion
        #region Properties
        public SupplierPayment SupplierPayment
        {
            get { return _supplierPayment; }
            set { SetProperty(ref _supplierPayment, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);
        #endregion
        #region Methods
        private void Save()
        {
            if (_edit && _supplierPaymentService.Update(SupplierPayment))
                _eventAggregator.GetEvent<SupplierPaymentUpdated>().Publish(SupplierPayment);
            else
                if (!_edit && _supplierPaymentService.Add(SupplierPayment))
                _eventAggregator.GetEvent<SupplierPaymentAdded>().Publish(SupplierPayment);
            else
                Messages.ErrorDataNotSaved();

            if (_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }
        private void Cancel()
        {
            if (_edit)
            {
                var c = _supplierPaymentService.Find(SupplierPayment.ID);
                DataUtils.Copy(SupplierPayment, c);
            }
            if (_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }
        #endregion
        #region Inteface
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            var supplier = (Supplier)(navigationContext.Parameters["Supplier"]);
            if (supplier == null)
            {
                SupplierPayment = (SupplierPayment)(navigationContext.Parameters["SupplierPayment"]);
                _edit = true;
            }
            else
                SupplierPayment = new SupplierPayment() { SupplierID = supplier.ID };

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var supplier = (Supplier)(navigationContext.Parameters["Supplier"]);
            if (supplier != null)
                return false;
            var supplierPayment2 = (SupplierPayment)(navigationContext.Parameters["SupplierPayment"]);
            return SupplierPayment.ID == supplierPayment2.ID;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion
        public SupplierPaymentEditViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ISupplierPaymentService supplierPaymentService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _supplierPaymentService = supplierPaymentService;
        }
    }
}
