using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Store
{
    public class StoreModule : IModule
    {
        IRegionManager _regionManager;

        public StoreModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
        }
    }
}