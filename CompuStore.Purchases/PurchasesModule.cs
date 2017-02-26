using CompuStore.Infrastructure;
using CompuStore.Purchases.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;

namespace CompuStore.Purchases
{
    public class PurchasesModule : IModule
    {
        readonly IUnityContainer _container;

        public PurchasesModule(RegionManager regionManager, IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterTypeForNavigation<PurchasesMain>(RegionNames.PurchasesMain);
            _container.RegisterTypeForNavigation<PurchaseEdit>(RegionNames.SupplierPurchaseEdit);
        }
    }
}