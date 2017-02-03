using CompuStore.Clients.Service;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Clients
{
    public class ClientsModule : IModule
    {
        IRegionManager RegionManager;
        IUnityContainer Container;

        public ClientsModule(RegionManager regionManager,IUnityContainer container)
        {
            RegionManager = regionManager;
            Container = container;
        }

        public void Initialize()
        {
            Container.RegisterType<IClientService, ClientService>(new ContainerControlledLifetimeManager());
        }
    }
}