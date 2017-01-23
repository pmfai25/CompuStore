using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Sales
{
    public class SalesModule : IModule
    {
        IRegionManager _regionManager;

        public SalesModule(IRegionManager regionManager )
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RequestNavigate(Infrastructure.RegionNames.MainContentRegion, Infrastructure.RegionNames.SalesMain);
        }
    }
}