using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Confirmations;
using Microsoft.Practices.Unity;
using Model;
using System;
using Prism.Commands;
using Model.Events;
using Service;

namespace CompuStore.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private bool hideUsers;
        private bool hideReports;
        private bool hideSuppliers;
        public bool HideSuppliers
        {
            get { return hideSuppliers; }
            set { SetProperty(ref hideSuppliers, value); }
        }
        public bool HideUsers
        {
            get { return hideUsers; }
            set { SetProperty(ref hideUsers, value); }
        }
        public bool HideReports
        {
            get { return hideReports; }
            set { SetProperty(ref hideReports, value); }
        }
        public InteractionRequest<LoginConfirmation> LoginRequest { get; set; }
        public InteractionRequest<RegisterConfirmation> RegisterRequest { get; set; }
        public DelegateCommand ShowLoginCommand => new DelegateCommand(ShowLogin);
        private void ShowRegister(RegisterValues reg)
        {
            RegisterRequest.Raise(new RegisterConfirmation(reg.Challenge, reg.Serial), x =>
              {
                  if (!x.Confirmed)
                  {
                      App.Current.Shutdown();
                      return;
                  }
                  ISettingsService service = _container.Resolve<ISettingsService>();
                  var set = service.Get();
                  set.Serial = x.Serial;
                  service.Update(set);
              });
        }
        private void ShowLogin()
        {
            LoginRequest.Raise(new LoginConfirmation(), x =>
            {
                if (!x.Confirmed)
                {
                    App.Current.Shutdown();
                    return;
                }
                _container.RegisterInstance<Account>(x.SelectedAccount, new ContainerControlledLifetimeManager());
                HideReports = HideSuppliers = HideUsers = x.SelectedAccount.ID != 1;
                isAdmin = !HideReports;
                OnPropertyChanged(() => Index);
                Index = isAdmin ? 0 : 1;
            });
        }

        private int index;
        public int Index
        {
            get { return index; }
            set {
                SetProperty(ref index, value);
                switch(index)
                {
                    case 0:
                        _regionManager.RequestNavigate("SuppliersRegion", "SuppliersMain");
                        break;
                    case 1:
                        _regionManager.RequestNavigate("ClientsRegion", "ClientsMain");
                        break;
                    case 2:
                        _regionManager.RequestNavigate("StoreRegion", "StoreMain");
                        break;
                    case 3:
                        _regionManager.RequestNavigate("ReportsRegion", "ReportsMain");
                        break;
                    case 4:
                        _regionManager.RequestNavigate("UsersRegion", "UsersMain");
                        break;

                }
            }
        }
        public IRegionManager _regionManager;
        private bool isAdmin;
        private IUnityContainer _container;

        public ShellViewModel(IRegionManager regionManager, IUnityContainer container,IEventAggregator eventAggregator)
        {
            _container = container;
            _regionManager = regionManager;
            LoginRequest = new InteractionRequest<LoginConfirmation>();
            RegisterRequest = new InteractionRequest<RegisterConfirmation>();
            eventAggregator.GetEvent<DoLogin>().Subscribe(ShowLogin);
            eventAggregator.GetEvent<DoRegister>().Subscribe(ShowRegister);
        }
    }
}
