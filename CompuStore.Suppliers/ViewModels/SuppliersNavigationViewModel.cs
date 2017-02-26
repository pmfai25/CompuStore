using CompuStore.Infrastructure;
using Model;
using Model.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Suppliers.ViewModels
{
    public class SuppliersNavigationViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private ISupplierService _supplierService;
        private IRegionManager _regionManager;
        private Supplier _selectedItem;
        private string _searchText;
        private ObservableCollection<Supplier> items;


        public Supplier SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<Supplier> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        private void Add()
        {
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierEdit);
        }
        private void Update()
        {
            var parameters = new NavigationParameters { { "Supplier", SelectedItem } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.SupplierEdit, parameters);
        }
        private void Delete()
        {
            if (_supplierService.IsSupplierWithPurchases(SelectedItem))
                Messages.Error("لايمكن حذف شركة لها عمليات شراء الا بعد حذف المشتريات اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name)) return;
                _supplierService.Delete(SelectedItem);
                Items.Remove(SelectedItem);
                _eventAggregator.GetEvent<SupplierDeleted>().Publish(SelectedItem);
            }
        }
        private void Search()
        {
            Items = new ObservableCollection<Supplier>(_supplierService.SearchBy(SearchText));
        }
        public SuppliersNavigationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ISupplierService supplierService)
        {
            _searchText = "";
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _supplierService = supplierService;
            Items = new ObservableCollection<Supplier>(_supplierService.GetAll(false));
            SelectedItem = Items.FirstOrDefault();
        }
    }
}
