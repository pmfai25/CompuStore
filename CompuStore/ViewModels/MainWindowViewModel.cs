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
        public DelegateCommand SalesCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SalesMain));
        public DelegateCommand PurchasesCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.PurchasesMain));
        public DelegateCommand StoreCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.StoreMain));
        public DelegateCommand ClientsCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientsMain));
        public DelegateCommand SuppliersCommand => new DelegateCommand(()=>this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SuppliersMain));
        public DelegateCommand ReportsCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ReportsMain));

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;
        }
    }
}
