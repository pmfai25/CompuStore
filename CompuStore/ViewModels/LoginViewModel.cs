using CompuStore.Confirmations;
using CompuStore.Infrastructure;
using Microsoft.Practices.Unity;
using Model;
using Model.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CompuStore.ViewModels
{
    public class LoginViewModel : BindableBase, IInteractionRequestAware, IDataErrorInfo
    {
        private LoginConfirmation _confirmation;
        private ObservableCollection<Account> _accounts;
        private Account _selectedAccount;
        private IAccountService _accountService;
        private Color color;
        public Color Color
        {
            get { return color; }
            set { SetProperty(ref color, value); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public DelegateCommand<object> LoginCommand => new DelegateCommand<object>(LoginUser);
        public DelegateCommand<KeyEventArgs> PasswordEnterCommand => new DelegateCommand<KeyEventArgs>(e=> { if (e.Key != Key.Enter) return; LoginUser(e.Source); });
        private void LoginUser(object password)
        {
            PasswordBox box = (PasswordBox)password;
            string pass = box.Password;
            successful = SelectedAccount.Password == pass;
            if (!successful)
            {
                box.Background = Brushes.Red;
                return;
            }
            successful = true;
            Color = Colors.White;
            _confirmation.Confirmed = true;
            _confirmation.SelectedAccount = SelectedAccount;
            FinishInteraction();
        }

        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }        
        private bool successful;
        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set { SetProperty(ref _selectedAccount, value); }
        }
        public INotification Notification
        {
            get
            {
                return _confirmation;
            }

            set
            {
                _confirmation = (LoginConfirmation)value;
                OnPropertyChanged(() => Notification);
            }
        }

        public Action FinishInteraction
        {
            get;set;
            
        }

        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (columnName == "Password")
                {
                    if (Password != SelectedAccount.Password)
                        return "كلمة مرور خاطئة";
                }
                return null;
            }
        }
        public LoginViewModel()
        {
            _accountService = new AccountService();
            Accounts = new ObservableCollection<Account>(_accountService.GetAll());
            SelectedAccount = Accounts.First();
            Color = Colors.White;
        }
    }
}
