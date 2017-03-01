using Model;
using Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using CompuStore.Infrastructure;
using Prism.Events;
using Model.Events;

namespace CompuStore.Clients.ViewModels
{
    public class ClientEditViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        #region Fields
        private bool _edit;
        private Client _client;
        private readonly IClientService _clientService;
        private readonly IEventAggregator _eventAggregator;
        private NavigationContext _navigationContext;
        #endregion
        #region Properties
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);
        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }
        #endregion
        #region Interface
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Client = (Client)(navigationContext.Parameters["Client"]) ?? new Client();
            _navigationContext = navigationContext;
            _edit = Client.ID != 0;            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        #endregion
        #region Methods
        private void Save()
        {
            if (CanSave())
            {
                if (_edit && _clientService.Update(Client))
                    _eventAggregator.GetEvent<ClientUpdated>().Publish(Client);
                else if (!_edit && _clientService.Add(Client))
                    _eventAggregator.GetEvent<ClientAdded>().Publish(Client);
                else
                {
                    Messages.ErrorDataNotSaved();
                    return;
                }
                _navigationContext.NavigationService.Journal.GoBack();               
            }
            else
                Messages.Error("يجب ادخال اسم ورقم تليفون للعميل");
            
        }
        private bool CanSave()
        {
            return !(String.IsNullOrWhiteSpace(Client.Name) || String.IsNullOrWhiteSpace(Client.Phone));
        }
        private void Cancel()
        {
            if (_edit)
            {
                var c2 = _clientService.Find(Client.ID);
                DataUtils.Copy(Client, c2);
            }
            _navigationContext.NavigationService.Journal.GoBack();
        }
        
        #endregion
        public ClientEditViewModel(IClientService clientService,  IEventAggregator eventAggregator)
        {
            _clientService = clientService;
            _eventAggregator = eventAggregator;            
        }

    }
}
