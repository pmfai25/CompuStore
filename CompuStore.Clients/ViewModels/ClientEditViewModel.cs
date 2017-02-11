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
    public class ClientEditViewModel : BindableBase, INavigationAware,IConfirmNavigationRequest
    {
        #region Fields
        private bool _saved;
        private bool _edit;
        private Client _client;
        private readonly IRegionManager _regionManager;
        private IClientService _clientService;
        private readonly IEventAggregator _eventAggregator;
        #endregion
        #region Properties
        public Client Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);
        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        public IClientService ClientService
        {
            get
            {
                return _clientService;
            }

            set
            {
                _clientService = value;
            }
        }
        #endregion
        #region Interface
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            int id =(int)( navigationContext.Parameters["ID"] ?? 0);
            Client = id == 0 ? new Client() : ClientService.Find(id);
            _edit = id != 0;            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if (_saved)
                continuationCallback(true);
            else
                continuationCallback(MessageBox.Show("الغاء التعديلات?","Question", MessageBoxButton.OKCancel) == MessageBoxResult.OK);
        }
        #endregion
        #region Methods
        private void Save()
        {
            if (CanSave())
            {
                if (_edit)
                    _saved = this.ClientService.Update(Client);
                else
                    _saved = ClientService.Add(Client);
                if (!_saved)
                {
                    Messages.Error("حدث خطأ اثناء حفظ العميل في قاعدة البيانات");
                    return;
                }
                _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientsMain);
                _eventAggregator.GetEvent<ClientAddedOrUpdated>().Publish(Client);
            }
            else
                Messages.Error("يجب ادخال اسم ورقم تليفون للعميل");
            
        }
        private void Cancel()
        {
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.ClientsMain);
        }
        private bool CanSave()
        {
            return !(String.IsNullOrWhiteSpace(Client.Name) || String.IsNullOrWhiteSpace(Client.Phone));            
        }
        #endregion
        public ClientEditViewModel(IClientService clientService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this._regionManager = regionManager;
            this.ClientService = clientService;
            this._eventAggregator = eventAggregator;            
        }

    }
}
