using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Suppliers
{
    public class SuppliersModule : IModule
    {
        IRegionManager _regionManager;

        public SuppliersModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}