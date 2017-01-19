using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Store
{
    public class StoresModule : IModule
    {
        IRegionManager _regionManager;

        public StoresModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            this._regionManager.RegisterViewWithRegion(CompuStore.Infrastructure.RegionNames.MainContentRegion, typeof(Views.StoresMain));
        }
    }
}