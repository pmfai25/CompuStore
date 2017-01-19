using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Suppliers
{
    public class PurchasesModule : IModule
    {
        IRegionManager _regionManager;

        public PurchasesModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            this._regionManager.RegisterViewWithRegion(Infrastructure.RegionNames.MainContentRegion, typeof(Views.PurchasesMain));

        }
    }
}