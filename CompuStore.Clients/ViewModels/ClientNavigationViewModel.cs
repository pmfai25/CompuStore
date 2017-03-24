using CompuStore.Clients.Confirmations;
using CompuStore.Infrastructure;
using Model;
using Model.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Clients.ViewModels
{
    public class ClientNavigationViewModel : BindableBase
    {
        #region Fields
        private Client _selectedItem;
        private string _searchText;
        private ObservableCollection<Client> _items;
        private IClientService _clientService;
        private IEventAggregator _eventAggregator;
        #endregion
        #region Properties
        public InteractionRequest<ClientConfirmation> ClientEditRequest { get; set; }
        public Client SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                bool skip = false;
                if ((SelectedItem != null && value != null && SelectedItem.ID == value.ID))
                    skip = true;
                SetProperty(ref _selectedItem, value);
                if (!skip)
                    _eventAggregator.GetEvent <ClientSelected>().Publish(SelectedItem);
            }
        }
        public ObservableCollection<Client> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); SelectedItem = Items.FirstOrDefault(); }
        }
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        #endregion
        #region Methods
        private void Add()
        {
            ClientEditRequest.Raise(new ClientConfirmation(), x =>
             {
                 if (x.Confirmed)
                 {
                     if (_clientService.Add(x.Client))
                         Items.Add(x.Client);
                     else
                         Messages.Error("يوجد عميل بنفس رقم التليفون");
                 }
             });
           
        }
        private void Update()
        {
            ClientEditRequest.Raise(new ClientConfirmation(SelectedItem), x =>
            {
                if (!x.Confirmed)
                {
                    DataUtils.Copy(SelectedItem, _clientService.Find(SelectedItem.ID));
                    return;
                }
                if (!_clientService.Update(x.Client))
                {
                    Messages.Error("يوجد عميل بنفس رقم التليفون");
                    DataUtils.Copy(SelectedItem, _clientService.Find(SelectedItem.ID));
                }
            });
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (!_clientService.IsDeletable(SelectedItem))
                Messages.Error("لايمكن حذف عميل له عمليات بيع الا بعد حذف المبيعات اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name)) return;
                _clientService.Delete(SelectedItem);
                Items.Remove(SelectedItem);
            }
        }
        private void Search()
        {
            Items = new ObservableCollection<Client>(_clientService.SearchBy(SearchText));
        }
        #endregion
        public ClientNavigationViewModel(IClientService clientService,IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _searchText = "";
            _clientService = clientService;
            ClientEditRequest = new InteractionRequest<ClientConfirmation>();
            Items = new ObservableCollection<Client>(_clientService.GetAll());
        }
    }
}
