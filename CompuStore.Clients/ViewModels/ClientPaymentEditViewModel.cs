using Model;
using Service;
using CompuStore.Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Model.Events;
using System;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Clients.Confirmations;

namespace CompuStore.Clients.ViewModels
{
    public class ClientPaymentEditViewModel : BindableBase, IInteractionRequestAware
    {
        private ClientPaymentConfirmation _confirmation;
        private ClientPayment _clientPayment;
        public ClientPayment ClientPayment
        {
            get { return _clientPayment; }
            set { SetProperty(ref _clientPayment, value); }
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
                _confirmation = value as ClientPaymentConfirmation;
                ClientPayment = _confirmation.ClientPayment;
                OnPropertyChanged(() => Notification);
            }
        }
        public Action FinishInteraction
        {
            set;get;
        }
        private void Save()
        {
            _confirmation.Confirmed = true;
            FinishInteraction();
        }     
    }
}
