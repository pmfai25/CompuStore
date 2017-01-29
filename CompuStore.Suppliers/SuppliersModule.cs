using CompuStore.Suppliers.Service;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Suppliers
{
    public class SuppliersModule : IModule
    {
        private IUnityContainer Container;
        IRegionManager _regionManager;

        public SuppliersModule(RegionManager regionManager, IUnityContainer Container)
        {
            _regionManager = regionManager;
            this.Container = Container;
        }

        public void Initialize()
        {
            Container.RegisterType<ISuppliersService, SupplierService>(new ContainerControlledLifetimeManager());
        }
    }
}