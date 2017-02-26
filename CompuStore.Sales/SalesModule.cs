using CompuStore.Infrastructure;
using CompuStore.Sales.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;

namespace CompuStore.Sales
{
    public class SalesModule : IModule
    {
        IUnityContainer _container;
        IRegionManager regionManager;
        public SalesModule(IUnityContainer container, IRegionManager regionManager )
        {
            this.regionManager = regionManager;
            _container = container;
        }
        public void Initialize()
        {
            _container.RegisterTypeForNavigation<SalesMain>(RegionNames.SalesMain);
            _container.RegisterTypeForNavigation<SalesEdit>(RegionNames.SalesEdit);           
        }
    }
}