using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Model.Events;
using System.Windows.Input;

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
        private IEventAggregator _eventAggregator;
        #endregion
        #region Properties
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); Refresh(); }
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
        public DelegateCommand<KeyEventArgs> SearchCommand => new DelegateCommand<KeyEventArgs>(Search);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh);


        #endregion
        private void Refresh()
        {
            if (SelectedCategory != null)
                Items = new ObservableCollection<Item>(_itemService.GetAll(SelectedCategory.ID));
            else
                Items = new ObservableCollection<Item>();
        }
        private void Add()
        {
            if(Categories.Count==0)
            {
                Messages.Error("يجب اضافة انواع الاصناف اولا");
                return;
            }
            NavigationParameters parameter = new NavigationParameters();
            parameter.Add("Categories", Categories);
            parameter.Add("SelectedCategory", SelectedCategory);
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.StoreEdit,parameter);
        }
        private void Update()
        {
            NavigationParameters parameter = new NavigationParameters();
            parameter.Add("Item", SelectedItem);
            parameter.Add("Categories", Categories);
            parameter.Add("SelectedCategory", SelectedCategory);
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.StoreEdit, parameter);
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (!_itemService.IsDeletable(SelectedItem))
                Messages.Error("لايمكن حذف صنف له عمليات بيع او شراء الا بعد حذف المبيعات والمشتريات اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name)) return;
                _itemService.Delete(SelectedItem);
                Items.Remove(SelectedItem);
            }
        }
        private void Search(KeyEventArgs e)
        {
            
                if (e.Key != Key.Enter)
                return;
            long n;
            if (long.TryParse(SearchText, out n))
            {
                Items = new ObservableCollection<Item>(_itemService.SearchBy(n));
                SearchText = "";
            }
            else
                Items = new ObservableCollection<Item>(_itemService.SearchBy(SearchText));
        }
        private void EditCategories()
        {
            NavigationParameters parameters = new NavigationParameters { { "Categories", Categories } };
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, RegionNames.CategoryMain,parameters);
        }
        public StoreMainViewModel(IItemService itemService,ICategoryService categoryService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _itemService = itemService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ItemAdded>().Subscribe(OnItemAdded);
            _eventAggregator.GetEvent<ItemUpdated>().Subscribe(OnItemUpdated);
            _eventAggregator.GetEvent<PurchaseAdded>().Subscribe(x => Refresh());
            _eventAggregator.GetEvent<PurchaseDeleted>().Subscribe(x => Refresh());
            _eventAggregator.GetEvent<PurchaseUpdated>().Subscribe(x => Refresh());
            _eventAggregator.GetEvent<OrderAdded>().Subscribe(x => Refresh());
            _eventAggregator.GetEvent<OrderDeleted>().Subscribe(x => Refresh());
            _eventAggregator.GetEvent<OrderUpdated>().Subscribe(x => Refresh());
            Categories = new ObservableCollection<Category>(categoryService.GetAll());
            SelectedCategory = Categories.FirstOrDefault();
            Refresh();
        }

        private void OnItemUpdated(Item obj)
        {
            if (obj.CategoryID != SelectedCategory.ID)
                Items.Remove(obj);
        }

        private void OnItemAdded(Item obj)
        {
            if (obj.CategoryID == SelectedCategory.ID)
                Items.Add(obj);
        }
    }
}
