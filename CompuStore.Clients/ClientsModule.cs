using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Clients
{
    public class ClientsModule : IModule
    {
        IRegionManager _regionManager;

        public ClientsModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}