using CompuStore.Clients.Model;
using CompuStore.Clients.Service;
using CompuStore.Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Clients.ViewModels
{
    public class ClientPaymentEditViewModel : BindableBase,INavigationAware
    {
        #region Fields
        private bool _edit;
        private ClientPayment _clientPayment;
        private NavigationContext _navigationContext;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private IClientPaymentService _clientPaymentService;
        #endregion
        #region Properties
        public ClientPayment ClientPayment
        {
            get { return _clientPayment; }
            set { SetProperty(ref _clientPayment, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);
        #endregion
        #region Methods
        private void Save()
        {
            if (_edit && _clientPaymentService.Update(ClientPayment))
                _eventAggregator.GetEvent<ClientPaymentUpdated>().Publish(ClientPayment);
            else
                if (!_edit && _clientPaymentService.Add(ClientPayment))
                _eventAggregator.GetEvent<ClientPaymentAdded>().Publish(ClientPayment);
            else
                Messages.ErrorDataNotSaved();

            if (_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }
        private void Cancel()
        {
            if (_edit)
            {
                var c = _clientPaymentService.Find(ClientPayment.ID);
                DataUtils.Copy(ClientPayment,c);
            }
            if (_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }
        #endregion
        #region Inteface
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            int id = (int)(navigationContext.Parameters["ClientID"] ?? 0);
            if (id == 0)
            {
                ClientPayment = (ClientPayment)(navigationContext.Parameters["ClientPayment"]);
                _edit = true;
            }
            else
                ClientPayment = new ClientPayment() { ClientID = id };
                    
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        #endregion

        public ClientPaymentEditViewModel(IRegionManager regionManager,IEventAggregator eventAggregator,IClientPaymentService clientPaymentService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _clientPaymentService = clientPaymentService;
        }
    }
}
