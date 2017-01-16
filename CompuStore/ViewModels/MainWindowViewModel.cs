using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using CompuStore.Infrastructure;

namespace CompuStore.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "CompuStore Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public IRegionManager RegionManager;
        public DelegateCommand SuppliersCommand => new DelegateCommand(NavigateSuppliers);
        public DelegateCommand ClientsCommand => new DelegateCommand(NavigateClients);
        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;
        }
        private void NavigateClients()
        {
            var parameters = new NavigationParameters();
            parameters.Add("X", 1);
            this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientsMain,parameters);
        }
        private void NavigateSuppliers()
        {
            this.RegionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SuppliersMain);
        }       
    }
}
