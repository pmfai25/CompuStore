﻿using CompuStore.Infrastructure;
using Microsoft.Practices.Unity;
using Model;
using Model.Events;
using Prism.Commands;
using Prism.Events;
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
    public class LoginViewModel : BindableBase, IConfirmNavigationRequest,IDataErrorInfo
    {
        IUnityContainer _container;
        IRegionManager _regionManager;
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
        public DelegateCommand ExitCommand => new DelegateCommand(()=>Application.Current.Shutdown());
        public DelegateCommand<KeyEventArgs> PasswordEnterCommand => new DelegateCommand<KeyEventArgs>(e=> { if (e.Key != Key.Enter) return; LoginUser(e.Source); });
        private void LoginUser(object password)
        {
            PasswordBox box = (PasswordBox)password;
            string pass = box.Password;
            successful = SelectedAccount.Password == pass;
            if (!successful)
            {
                box.Background = System.Windows.Media.Brushes.Red;
                return;
            }
            successful = true;
            Color = Colors.White;
            _container.RegisterInstance<Account>(SelectedAccount,new ContainerControlledLifetimeManager());
            switch(SelectedAccount.ID)
            {
                case 1:                    
                    _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SuppliersMain);
                    break;
                default:
                    _eventAggregator.GetEvent<NormalUserLoggedIn>().Publish();
                    _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientsMain);

                    break;
            }
        }

        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }        
        private bool successful;
        private IEventAggregator _eventAggregator;

        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set { SetProperty(ref _selectedAccount, value); }
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
                if(columnName=="Password")
                {
                    if (Password != SelectedAccount.Password)
                        return "كلمة مرور خاطئة";
                }
                return null;
            }
        }

        public LoginViewModel(IUnityContainer container, IAccountService accountService,IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _container = container;
            _accountService = accountService;
            Accounts = new ObservableCollection<Account>(_accountService.GetAll());
            SelectedAccount = Accounts.First();
            Color = Colors.White;
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if (navigationContext.Uri == new Uri("RegisterNow",UriKind.Relative))
                continuationCallback(true);
            else
                continuationCallback(successful);
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
