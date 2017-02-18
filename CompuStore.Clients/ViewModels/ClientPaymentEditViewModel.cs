using Model;
using Service;
using CompuStore.Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using Model.Events;

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
            var client = (Client)(navigationContext.Parameters["Client"]);
            if (client == null)
            {
                ClientPayment = (ClientPayment)(navigationContext.Parameters["ClientPayment"]);
                _edit = true;
            }
            else
                ClientPayment = new ClientPayment() { ClientID = client.ID };
                    
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var client = (Client)(navigationContext.Parameters["Client"]);
            if (client != null)
                return false;
            var clientPayment2 = (ClientPayment)(navigationContext.Parameters["ClientPayment"]);
            return ClientPayment.ID == clientPayment2.ID;
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
