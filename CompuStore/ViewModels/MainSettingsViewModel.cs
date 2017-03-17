using CompuStore.Infrastructure;
using Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.ViewModels
{
    public class MainSettingsViewModel : BindableBase, INavigationAware
    {
        private List<Account> deleteList, updateList, insertList;
        private NavigationContext _navigationContext;
        private IAccountService _accountService;
        private Account _selectedItem;
        private ObservableCollection<Account> _items;
        public ObservableCollection<Account> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public Account SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public DelegateCommand AddCommand => new DelegateCommand(()=>Items.Add(new Account()));
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand CancelCommand => new DelegateCommand(()=>_navigationContext.NavigationService.Journal.GoBack());
        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        private void Save()
        {
            if(Items.Any(x=>!x.IsValid))
            {
                Messages.Error("يوجد اخطاء في بعض البيانات");
                return;
            }
            foreach (var i in Items)
                if (i.ID == 0)
                    insertList.Add(i);
                else
                    updateList.Add(i);
            insertList.ForEach(x => _accountService.AddAccount(x));
            updateList.ForEach(x => _accountService.UpdateAccount(x));
            deleteList.ForEach(x => _accountService.DeleteAccount(x));
            _navigationContext.NavigationService.Journal.GoBack();
        }

        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (SelectedItem.ID == 0)
            {
                Items.Remove(SelectedItem);
                return;
            }
            if(SelectedItem.ID==1)
            {
                Messages.Error("لا يمكن حذف مدير البرنامج");
                return;
            }
            deleteList.Add(SelectedItem);
            Items.Remove(SelectedItem);            
        }
        public MainSettingsViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            Items = new ObservableCollection<Account>(_accountService.GetAll());
            SelectedItem = Items.Single(x => x.ID == 1);
            deleteList = new List<Account>();
            updateList = new List<Account>();
            insertList = new List<Account>();
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
    }
}
