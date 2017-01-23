using CompuStore.Clients.Model;
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
        private Client selectedItem;
        public Client SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }
        private ObservableCollection<Client> items;
        public ObservableCollection<Client> Items
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
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand EditCommand => new DelegateCommand(Edit, CanEdit).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, CanDelete).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search).ObservesCanExecute(x => Items.Count > 0);
        private void Add()
        {
            items.Add(new Client() { Name = "Ali Mostafa", Address = "Alexandria", Phone = "1234213123" });
        }
        private void Edit()
        {
        }
        private bool CanEdit()
        {
            return selectedItem != null;
        }
        private void Delete()
        {
            items.Remove(selectedItem);
        }
        private bool CanDelete()
        {
            return selectedItem != null;
        }
        private void Search()
        {
        }


        public ClientsMainViewModel()
        {
            items = new ObservableCollection<Client>();
            items.Add(new Client() { Name = "Ahmed Mohammed", Address = "Cairo", Phone = "1234213123" });
            items.Add(new Client() { Name = "Mostafa Ahmed", Address = "Alex", Phone = "1234213123" });
        }
    }
}
