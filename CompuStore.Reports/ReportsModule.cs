using CompuStore.Infrastructure;
using CompuStore.Reports.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;

namespace CompuStore.Reports
{
    public class ReportsModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;

        public ReportsModule(RegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterTypeForNavigation<ReportsMain>(RegionNames.ReportsMain);
            _container.RegisterTypeForNavigation<ClientOrdersReport>(RegionNames.ClientOrdersReport);
            _container.RegisterTypeForNavigation<SupplierPurchasesReport>(RegionNames.SupplierPurchasesReport);
            _container.RegisterTypeForNavigation<IncomeOutcomeReport>(RegionNames.ReportIncomeOutcomeView);
        }
    }
}