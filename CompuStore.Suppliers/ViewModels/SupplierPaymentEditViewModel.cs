using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Model.Events;
using System;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPaymentEditViewModel : BindableBase, INavigationAware,IRegionMemberLifetime
    {
        #region Fields
        private bool _edit;
        private SupplierPayment _supplierPayment;
        private IEventAggregator _eventAggregator;
        private ISupplierPaymentService _supplierPaymentService;
        private Supplier _supplier;
        #endregion
        #region Properties
        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
        }
        public SupplierPayment SupplierPayment
        {
            get { return _supplierPayment; }
            set { SetProperty(ref _supplierPayment, value); }
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
            {
                Messages.ErrorDataNotSaved();
                return;
            }
            _navigationContext.NavigationService.Journal.GoBack();
        }
        private void Cancel()
        {
            if (_edit)
            {
                var c = _supplierPaymentService.Find(SupplierPayment.ID);
                DataUtils.Copy(SupplierPayment, c);
            }
            _navigationContext.NavigationService.Journal.GoBack();
        }
        #endregion
        #region Inteface
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Supplier = (Supplier)(navigationContext.Parameters["Supplier"]);
            SupplierPayment = (SupplierPayment)(navigationContext.Parameters["SupplierPayment"]) ?? new SupplierPayment() { SupplierID = Supplier.ID };
            _edit = SupplierPayment.ID != 0;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion
        public SupplierPaymentEditViewModel(IEventAggregator eventAggregator, ISupplierPaymentService supplierPaymentService)
        {
            _eventAggregator = eventAggregator;
            _supplierPaymentService = supplierPaymentService;
        }
    }
}
