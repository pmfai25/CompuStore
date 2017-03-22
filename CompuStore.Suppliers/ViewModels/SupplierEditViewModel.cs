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
        private SupplierConfirmation _confirmation { get; set; }
        private Supplier _supplier;
        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
        }        
        public DelegateCommand CancelCommand => new DelegateCommand(()=>FinishInteraction());
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public INotification Notification
        {
            get { return _confirmation; }

            set
            {
                _confirmation = (SupplierConfirmation)value;
                Supplier = _confirmation.Supplier;
                OnPropertyChanged(() => this.Notification);                
            }
        }
        public Action FinishInteraction
        {
            get;set;
        }
        private void Save()
        {
            if (!Supplier.IsValid)
                {
                    Messages.ErrorValidation();
                    return;
                }
            this._confirmation.Confirmed = true;
            this.FinishInteraction();
        }
    }
}
