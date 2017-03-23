using CompuStore.Infrastructure;
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
            _regionManager.RegisterViewWithRegion("SupplierPaymentRegion", typeof(SupplierPaymentMain));
            _regionManager.RegisterViewWithRegion("SupplierPurchasesRegion", typeof(SupplierPurchasesMain));
            _regionManager.RegisterViewWithRegion("SupplierNavigationRegion", typeof(SupplierNavigation));
            _regionManager.RegisterViewWithRegion("SuppliersRegion", typeof(SuppliersMain));     
        }
    }
}