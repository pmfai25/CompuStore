using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Model.Events;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPaymentEditViewModel : BindableBase, INavigationAware
    {
        #region Fields
        private bool _edit;
        private SupplierPayment _supplierPayment;
        private NavigationContext _navigationContext;
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
            var supplierPayment2 = (SupplierPayment)(navigationContext.Parameters["SupplierPayment"]);
            if (supplierPayment2 == null)
                return false;
            return SupplierPayment.ID == supplierPayment2.ID;
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
