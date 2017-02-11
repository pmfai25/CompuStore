using CompuStore.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Clients.ViewModels
{
    public class ClientPaymentEditViewModel : BindableBase,INavigationAware
    {
        public DelegateCommand BackCommand => new DelegateCommand(Back);
        NavigationContext _navigationContext;
        private IRegionManager _regionManager;

        private void Back()
        {
            if (_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public ClientPaymentEditViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

        }
    }
}
