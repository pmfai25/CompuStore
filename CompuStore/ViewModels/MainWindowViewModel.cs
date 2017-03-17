using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using CompuStore.Infrastructure;
using Prism.Events;

namespace CompuStore.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public IRegionManager RegionManager;
        public DelegateCommand ItemsCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.StoreMain));
        public DelegateCommand ClientsCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientsMain));
        public DelegateCommand SuppliersCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SuppliersMain));
        public DelegateCommand ReportsCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ReportsMain));
        public DelegateCommand SettingsCommand => new DelegateCommand(() => this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.Settings));
        private bool isUser;
        public bool IsAdmin
        {
            get { return isUser; }
            set { SetProperty(ref isUser, value); }
        }
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.RegionManager = regionManager;
            IsAdmin = true;
            eventAggregator.GetEvent<Model.Events.NormalUserLoggedIn>().Subscribe(()=>IsAdmin=false);
        }


    }
}
