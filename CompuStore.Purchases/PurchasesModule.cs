﻿using Prism.Modularity;
using Prism.Regions;
using System;

namespace CompuStore.Purchases
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
            

        }
    }
}