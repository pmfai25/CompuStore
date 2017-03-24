using CompuStore.Accounts.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;

namespace CompuStore.Accounts
{
    public class AccountsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public AccountsModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterTypeForNavigation<AccountsMain>("AccountsMain");
        }
    }
}