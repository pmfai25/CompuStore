using CompuStore.Infrastructure;
using Service;
using CompuStore.Suppliers.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace CompuStore.Suppliers
{
    public class SuppliersModule : IModule
    {
        private IUnityContainer _container;
        IRegionManager _regionManager;

        public SuppliersModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ISupplierService, SupplierService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISupplierPaymentService, SupplierPaymentService>(new ContainerControlledLifetimeManager());
            _container.RegisterTypeForNavigation<SuppliersMain>(RegionNames.SuppliersMain);
            _container.RegisterTypeForNavigation<SupplierEdit>(RegionNames.SupplierEdit);
            _container.RegisterTypeForNavigation<SupplierPaymentMain>(RegionNames.SupplierPaymentMain);
            _container.RegisterTypeForNavigation<SupplierPaymentEdit>(RegionNames.SupplierPaymentEdit);
            _container.RegisterTypeForNavigation<SupplierPurchasesMain>(RegionNames.SupplierPurchasesMain);
            _container.RegisterTypeForNavigation<SupplierPurchaseEdit>(RegionNames.SupplierPurchaseEdit);
            _regionManager.RegisterViewWithRegion(RegionNames.SuppliersRegion, typeof(SuppliersMain));     
        }
    }
}