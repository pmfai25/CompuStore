using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using Model.Events;
using System.Collections.Generic;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Suppliers.Confirmations;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierEditViewModel : BindableBase, IInteractionRequestAware
    {
        private Supplier _supplier;
        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
        }
        public SupplierEditConfirmation Confirmation { get; set; }
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);
        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        public INotification Notification
        {
            get
            {
                return Confirmation;
            }

            set
            {
                if(value is SupplierEditConfirmation)
                {
                    Confirmation = (SupplierEditConfirmation)value;
                    Supplier = Confirmation.Supplier;
                    OnPropertyChanged(() => this.Notification);
                }
            }
        }

        public Action FinishInteraction
        {
            get;set;
        }
        private void Cancel()
        {
            if (this.Confirmation != null)
                this.Confirmation.Confirmed = false;
            this.FinishInteraction();
        }
        private void Save()
        {
            if (this.Confirmation != null&&!this.Confirmation.Supplier.IsValid)
                {
                    Messages.ErrorValidation();
                    return;
                }
            this.Confirmation.Confirmed = true;
            this.Supplier = Supplier;
            this.FinishInteraction();
        }
    }
}
