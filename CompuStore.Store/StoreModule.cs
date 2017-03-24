using CompuStore.Infrastructure;
using Service;
using CompuStore.Store.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;

namespace CompuStore.Store
{
    public class StoreModule : IModule
    {
        IRegionManager _regionManager;
        readonly IUnityContainer _container;
        public StoreModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterTypeForNavigation<ItemsMain>("StoreMain");
        }
    }
}