using CompuStore.Clients.Model;
using CompuStore.Clients.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using CompuStore.Infrastructure;
using Prism.Events;

namespace CompuStore.Clients.ViewModels
{
    public class ClientEditViewModel : BindableBase, INavigationAware
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
            var c2= (Client)(navigationContext.Parameters["Client"]);
            if (c2 == null)
                return false;
            return c2.ID == Client.ID; 
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        #endregion
        #region Methods
        private void Save()
        {
            bool _saved;
            if (CanSave())
            {
                if (_edit)
                    _saved = _clientService.Update(Client);
                else
                    _saved = _clientService.Add(Client);
                if (!_saved)
                {
                    Messages.ErrorDataNotSaved();
                    return;
                }
                _eventAggregator.GetEvent<ClientAddedOrUpdated>().Publish(Client);
                _navigationContext.NavigationService.Journal.GoBack();               
            }
            else
                Messages.Error("يجب ادخال اسم ورقم تليفون للعميل");
            
        }
        private void Cancel()
        {
            var c2 = _clientService.Find(Client.ID);
            DataUtils.Copy(Client, c2);
            _navigationContext.NavigationService.Journal.GoBack();
        }
        private bool CanSave()
        {
            return !(String.IsNullOrWhiteSpace(Client.Name) || String.IsNullOrWhiteSpace(Client.Phone));            
        }
        #endregion
        public ClientEditViewModel(IClientService clientService,  IEventAggregator eventAggregator)
        {
            _clientService = clientService;
            _eventAggregator = eventAggregator;            
        }

    }
}
