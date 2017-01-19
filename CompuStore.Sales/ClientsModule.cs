using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Sales
{
    public class ClientsModule : IModule
    {
        IRegionManager _regionManager;

        public ClientsModule(IRegionManager regionManager )
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            
            this._regionManager.RegisterViewWithRegion(CompuStore.Infrastructure.RegionNames.MainContentRegion, typeof(Views.SalesMain));
        }
    }
}