using CompuStore.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Reports.ViewModels
{
    public class ReportsMainViewModel : BindableBase
    {
        private IRegionManager _regionManager;
        public DelegateCommand PurchasesCommand => new DelegateCommand(()=>_regionManager.RequestNavigate(RegionNames.MainContentRegion,RegionNames.SupplierPurchasesReport));
        public DelegateCommand OrdersCommand => new DelegateCommand(() => _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientOrdersReport));
        public ReportsMainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
