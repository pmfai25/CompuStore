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

namespace CompuStore.Store.ViewModels
{
    public class StoreEditViewModel : BindableBase,INavigationAware
    {
        private Item _item;
        private bool _edit;
        private NavigationContext _navigationContext;
        private IEventAggregator _eventAggregator;
        private IItemService _itemService;
        private ICategoryService _categoryService;
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }
        public Item Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }
        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        private void Save()
        {
            if (Item.IsValid)
            {
                Item.CategoryID = SelectedCategory.ID;
                if (_edit && _itemService.Update(Item))
                {
                    _eventAggregator.GetEvent<ItemUpdated>().Publish(Item);
                    _navigationContext.NavigationService.Journal.GoBack();
                }
                else
                    if (!_edit && _itemService.Add(Item))
                {
                    _eventAggregator.GetEvent<ItemAdded>().Publish(Item);
                    _navigationContext.NavigationService.Journal.GoBack();
                }
                else
                    Messages.ErrorDataNotSaved();

            }
            else
                Messages.Error("يجب ادخال بيانات صحيحة");
        }

        private bool CanSave()
        {
            return Item.Limit > -1 && Item.Price > 0 && !string.IsNullOrWhiteSpace(Item.Name) && Item.Serial > 0;
        }

        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);

        private void Cancel()
        {
            if(_edit)
            {
                var i2 = _itemService.Find(Item.ID);
                DataUtils.Copy(Item, i2);
            }
            _navigationContext.NavigationService.Journal.GoBack();
        }

        public StoreEditViewModel(IEventAggregator eventAggregator,ICategoryService categoryService, IItemService itemService)
        {
            _eventAggregator = eventAggregator;
            _categoryService = categoryService;            
            _itemService = itemService;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var i2 = (Item)navigationContext.Parameters["Item"];
            if (i2 == null)
                return false;
            return i2.ID == _item.ID;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Categories = (ObservableCollection<Category>)navigationContext.Parameters["Categories"] ?? new ObservableCollection<Category>(_categoryService.GetAll());
            SelectedCategory = (Category)navigationContext.Parameters["SelectedCategory"] ?? Categories.First();
            Item = (Item)navigationContext.Parameters["Item"] ?? new Item() { CategoryID = SelectedCategory.ID };
            Item.Items = new List<Item>(_itemService.GetAll(true));
            _edit = Item.ID != 0;
        }
    }
}
