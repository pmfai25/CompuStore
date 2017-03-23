using Model;
using Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using CompuStore.Infrastructure;
using Prism.Events;
using Model.Events;
using System.Collections.Generic;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Clients.Confirmations;

namespace CompuStore.Clients.ViewModels
{
    public class ClientEditViewModel : BindableBase, IInteractionRequestAware
    {
        private ClientConfirmation _confirmation;
        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
        public DelegateCommand CancelCommand => new DelegateCommand(()=>FinishInteraction());
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public INotification Notification
        {
            get
            {
                return _confirmation;
            }

            set
            {
                _confirmation = (ClientConfirmation)value;
                Client = _confirmation.Client;
                OnPropertyChanged(() => Notification);
            }
        }
        public Action FinishInteraction
        {
            set;get;
        }       
        private void Save()
        {
            if (!Client.IsValid)
            {
                Messages.ErrorValidation();
                return;
            }
            _confirmation.Confirmed = true;
            FinishInteraction();
        }
    }
}
