using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Reports
{
    public class ReportsModule : IModule
    {
        IRegionManager _regionManager;

        public ReportsModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
        }
    }
}