using CompuStore.Clients.Service;
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
        readonly IUnityContainer _container;

        public ClientsModule(RegionManager regionManager,IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<IClientService, ClientService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IClientPaymentService, ClientPaymentService>(new ContainerControlledLifetimeManager());

            _container.RegisterTypeForNavigation<ClientsMain>(RegionNames.ClientsMain);
            _container.RegisterTypeForNavigation<ClientEdit>(RegionNames.ClientEdit);
            _container.RegisterTypeForNavigation<ClientPaymentMain>(RegionNames.ClientPaymentMain);
            _container.RegisterTypeForNavigation<ClientPaymentEdit>(RegionNames.ClientPaymentEdit);
            _container.RegisterTypeForNavigation<ClientSalesMain>(RegionNames.ClientSalesMain);
        }
    }
}