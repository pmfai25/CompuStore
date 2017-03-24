using CompuStore.Accounts.Confirmations;
using CompuStore.Infrastructure;
using Model;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Accounts.ViewModels
{
    public class AccountsMainViewModel : BindableBase
    {
        private decimal _total;
        private decimal _money;
        private decimal hund = 100;
        private decimal percent;
        
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private IAccountService _accountService;
        private List<Orders> _sales;
        public List<Orders> Sales
        {
            get { return _sales; }
            set { SetProperty(ref _sales, value); }
        }
        private Account _selectedItem;
        private ObservableCollection<Account> _items;
        public decimal Total
        {
            get { return _total; }
            set
            {
                SetProperty(ref _total, value);
                Money=(Percent*Total)/hund;
            }
        }
        public decimal Money
        {
            get { return _money; }
            set { SetProperty(ref _money, value); }
        }
        public decimal Percent
        {
            get { return percent; }
            set {
                SetProperty(ref percent, value);
                Money = (Percent * Total) / hund;
            }
        }
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }
        public ObservableCollection<Account> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public Account SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); Search(); }
        }
        public InteractionRequest<AccountConfirmation> AccountEditRequest { set; get; }
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, ()=>SelectedItem!=null).ObservesProperty(()=>SelectedItem);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand SearchCommand => new DelegateCommand(Search,() => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        private void Refresh()
        {
            if (SelectedItem == null)
                return;
            Sales = _accountService.GetSales(SelectedItem);
            Total =Sales.Sum(x=>x.Total);
        }
        private void Search()
        {
            if (SelectedItem == null)
                return;
            Sales = _accountService.GetSales(SelectedItem, DateFrom, DateTo);
            Total = Sales.Sum(x => x.Total);
        }
        private void Add()
        {
            AccountEditRequest.Raise(new AccountConfirmation(), x =>
             {
                 if (x.Confirmed)
                 {
                     _accountService.AddAccount(x.Account);
                     Items.Add(x.Account);
                     Search();
                 }
             });
        }
        private void Update()
        {
            AccountEditRequest.Raise(new AccountConfirmation(SelectedItem), x =>
            {
                if (x.Confirmed)
                    _accountService.UpdateAccount(x.Account);
                else
                    DataUtils.Copy(SelectedItem, _accountService.Find(SelectedItem.ID));
            });
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
           if(SelectedItem.ID==1)
            {
                Messages.Error("لا يمكن حذف مدير البرنامج");
                return;
            }
            if (Messages.Delete(SelectedItem.Username))
            {
                _accountService.DeleteAccount(SelectedItem);
                Items.Remove(SelectedItem);
                Search();
            }
        }
        public AccountsMainViewModel(IAccountService accountService)
        {
            DateFrom = DateTime.Now.AddDays(-DateTime.Now.Day+1);
            DateTo = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.Day);
            _accountService = accountService;
            Items = new ObservableCollection<Account>(_accountService.GetAll());
            AccountEditRequest = new InteractionRequest<AccountConfirmation>();
            Percent = 1;
            SelectedItem = Items.First();
        }
    }
}
