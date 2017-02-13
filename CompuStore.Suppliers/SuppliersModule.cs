using CompuStore.Infrastructure;
using CompuStore.Suppliers.Service;
using CompuStore.Suppliers.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

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
            Container.RegisterType<ISupplierService, SupplierService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISupplierPaymentService, SupplierPaymentService>(new ContainerControlledLifetimeManager());
            Container.RegisterTypeForNavigation<SuppliersMain>(RegionNames.SuppliersMain);
            Container.RegisterTypeForNavigation<SupplierEdit>(RegionNames.SupplierEdit);
            Container.RegisterTypeForNavigation<SupplierPaymentMain>(RegionNames.SupplierPaymentMain);
            Container.RegisterTypeForNavigation<SupplierPaymentEdit>(RegionNames.SupplierPaymentEdit);
            Container.RegisterTypeForNavigation<SupplierPurchasesMain>(RegionNames.SupplierPurchasesMain);
        }
    }
}