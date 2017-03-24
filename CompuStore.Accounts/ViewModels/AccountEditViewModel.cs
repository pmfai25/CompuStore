using CompuStore.Accounts.Confirmations;
using CompuStore.Infrastructure;
using Model;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Accounts.ViewModels
{
    public class AccountEditViewModel : BindableBase ,IInteractionRequestAware
    {
        private AccountConfirmation _confirmation;
        private Account _account;
        public Account Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        public INotification Notification
        {
            get
            {
                return _confirmation;
            }

            set
            {
                _confirmation = (AccountConfirmation)value;
                Account = _confirmation.Account;
                OnPropertyChanged(() => Notification);
            }
        }

        public Action FinishInteraction
        {
            get;set;
        }
        public DelegateCommand CancelCommand => new DelegateCommand(() => FinishInteraction());
        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        private void Save()
        {
            if(!Account.IsValid)
            {
                Messages.ErrorValidation();
                return;
            }
            _confirmation.Confirmed = true;
            FinishInteraction();
        }
        public AccountEditViewModel()
        {
            Account = new Account();
        }
    }
}
