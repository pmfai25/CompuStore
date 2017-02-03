using CompuStore.Clients.Model;
using CompuStore.Clients.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Clients.ViewModels
{
    public class ClientsMainViewModel : BindableBase
    {
        #region Fields
        private IClientService clientService;
        private ClientsDetails selectedItem;
        public ClientsDetails SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }
        private ObservableCollection<ClientsDetails> items;
        public ObservableCollection<ClientsDetails> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { SetProperty(ref searchText, value); }
        }
        #endregion
        #region Commands
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand EditCommand => new DelegateCommand(Edit, CanEdit).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, CanDelete).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search).ObservesCanExecute(x => Items.Count > 0);
        private bool CanDelete()
        {
            return selectedItem != null;
        }
        private bool CanEdit()
        {
            return selectedItem != null;
        }
        #endregion
        private void Add()
        {
            
        }
        private void Edit()
        {
        }
       
        private void Delete()
        {
            clientService.Delete(SelectedItem.ID);
            items.Remove(SelectedItem);
        }
       
        private void Search()
        {
            items = new ObservableCollection<ClientsDetails>(clientService.SearchBy(SearchText));
        }


        public ClientsMainViewModel(IClientService clientService)
        {
            this.clientService = clientService;
            items = new ObservableCollection<ClientsDetails>();
            items.Add(new ClientsDetails() { Name = "Ahmed Mohammed", Payments=5000, Initial=1000,Sales=7000, Phone = "1234213123" });
            items.Add(new ClientsDetails() { Name = "Ahmed Mohammed", Payments = 5000, Initial = 1000, Sales = 7000, Phone = "1234213123" });
        }
    }
}
