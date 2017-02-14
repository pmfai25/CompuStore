using CompuStore.Infrastructure;
using CompuStore.Store.Model;
using CompuStore.Store.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Store.ViewModels
{
    public class StoreMainViewModel : BindableBase
    {
        #region Fields
        private readonly IRegionManager _regionManager;
        private IItemService _itemService;
        private string _searchText;
        private Item _selectedItem;
        private ObservableCollection<Item> _items;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;
        #endregion
        #region Properties
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
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
        public DelegateCommand EditCategoriesCommand => new DelegateCommand(EditCategories);
        public DelegateCommand SearchCommand => new DelegateCommand(SearchItems);       
        #endregion
        private void Add()
        {
        }
        private void Update()
        {
        }
        private void Delete()
        {
            if (!_itemService.IsDeletable(SelectedItem))
                Messages.Error("لايمكن حذف صنف له عمليات بيع او شراء الا بعد حذف المبيعات والمشتريات اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name)) return;
                _itemService.Delete(SelectedItem);
                Items.Remove(SelectedItem);
            }
        }
        private void SearchItems()
        {
            Items = new ObservableCollection<Item>(_itemService.GetAll(SelectedCategory.ID));
        }
        private void EditCategories()
        {
        }
        public StoreMainViewModel(IItemService itemService,ICategoryService categoryService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _itemService = itemService;
            _regionManager = regionManager;
            _searchText = "";
            Categories = new ObservableCollection<Model.Category>(categoryService.GetAll());
            SelectedCategory = Categories.FirstOrDefault();
        }
    }
}
