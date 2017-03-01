using Model;
using Service;
using CompuStore.Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Model.Events;
using System;

namespace CompuStore.Clients.ViewModels
{
    public class ClientPaymentEditViewModel : BindableBase,INavigationAware,IRegionMemberLifetime
    {
        #region Fields
        private bool _edit;
        private ClientPayment _clientPayment;
        private NavigationContext _navigationContext;
        private IEventAggregator _eventAggregator;
        private IClientPaymentService _clientPaymentService;
        private Client _client;
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
        #endregion
        #region Properties
        public ClientPayment ClientPayment
        {
            get { return _clientPayment; }
            set { SetProperty(ref _clientPayment, value); }
        }
        public bool KeepAlive
        {
            get
            {
                return false;
            }
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
            {
                Messages.ErrorDataNotSaved();
                return;
            }
            _navigationContext.NavigationService.Journal.GoBack();
        }
        private void Cancel()
        {
            if (_edit)
            {
                var c = _clientPaymentService.Find(ClientPayment.ID);
                DataUtils.Copy(ClientPayment,c);
            }
            _navigationContext.NavigationService.Journal.GoBack();
        }
        #endregion
        #region Inteface
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Client = (Client)(navigationContext.Parameters["Client"]);
            ClientPayment = (ClientPayment)(navigationContext.Parameters["ClientPayment"])??new ClientPayment() { ClientID = Client.ID };
            _edit = ClientPayment.ID != 0;     
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        #endregion
        public ClientPaymentEditViewModel(IEventAggregator eventAggregator,IClientPaymentService clientPaymentService)
        {
            _eventAggregator = eventAggregator;
            _clientPaymentService = clientPaymentService;
        }
    }
}
