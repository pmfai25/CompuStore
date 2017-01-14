using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Items
{
    public class ItemsModule : IModule
    {
        IRegionManager _regionManager;

        public ItemsModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}