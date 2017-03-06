using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using CompuStore.Infrastructure;

namespace CompuStore.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public IRegionManager RegionManager;
        public DelegateCommand SalesCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientOrdersReport));
        public DelegateCommand PurchasesCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierPurchasesReport));
        public DelegateCommand StoreCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.StoreMain));
        public DelegateCommand ClientsCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientsMain));
        public DelegateCommand SuppliersCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SuppliersMain));
        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;
            
        }
    }
}
