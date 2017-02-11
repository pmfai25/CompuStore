using CompuStore.Clients.Service;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

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
        }
    }
}