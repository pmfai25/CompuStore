using Microsoft.Practices.Unity;
using Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.ObjectModel;

namespace CompuStore.ViewModels
{
    public class LoginViewModel : BindableBase, IConfirmNavigationRequest
    {
        IUnityContainer _container;
        private ObservableCollection<Account> _accounts;
        private IAccountService _accountService;
        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public DelegateCommand LoginCommand => new DelegateCommand(LoginUser,()=>!string.IsNullOrWhiteSpace(Password)).ObservesProperty(()=>Password);

        private void LoginUser()
        {
            successful = SelectedAccount.Password == Password;
            if (!successful)
                return;
            _container.RegisterInstance<Account>(SelectedAccount,new ContainerControlledLifetimeManager());
            switch(SelectedAccount.Role)
            {
                case 0:

                    break;
                case 1:
                    break;
            }
        }

        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }
        private Account _selectedAccount;
        private bool successful;

        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set { SetProperty(ref _selectedAccount, value); }
        }
        public LoginViewModel(IUnityContainer container, IAccountService accountService)
        {
            _container = container;
            _accountService = accountService;
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if(!successful)
                continuationCallback(false);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }
    }
}
