using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Model.Events;
using System;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Suppliers.Confirmations;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierPaymentEditViewModel : BindableBase, IInteractionRequestAware
    {
        private SupplierPaymentConfirmation _confirmation;
        private SupplierPayment _supplierPayment;
        public SupplierPayment SupplierPayment
        {
            get { return _supplierPayment; }
            set { SetProperty(ref _supplierPayment, value); }
        }
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public DelegateCommand CancelCommand => new DelegateCommand(()=>FinishInteraction());
        public INotification Notification
        {
            get
            {
                return _confirmation;
            }

            set
            {
                _confirmation = (SupplierPaymentConfirmation)value;
                SupplierPayment = _confirmation.SupplierPayment;
                OnPropertyChanged(() => this.Notification);
            }
        }
        public Action FinishInteraction
        {
            get;set;
        }
        private void Save()
        {
            _confirmation.Confirmed = true;
            FinishInteraction();
        }
    }
}
