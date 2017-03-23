using Service;
using CompuStore.Clients.Views;
using CompuStore.Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace CompuStore.Clients
{
    public class ClientsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ClientsModule(RegionManager regionManager,IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            
            _regionManager.RegisterViewWithRegion("ClientPaymentRegion", typeof(ClientPaymentMain));
            _regionManager.RegisterViewWithRegion("ClientSalesRegion", typeof(ClientSalesMain));
            _regionManager.RegisterViewWithRegion("ClientNavigationRegion", typeof(ClientNavigation));
            _regionManager.RegisterViewWithRegion("ClientsRegion", typeof(ClientsMain));
        }
    }
}