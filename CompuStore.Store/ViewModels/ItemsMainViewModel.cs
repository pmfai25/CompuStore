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
using Prism.Interactivity.InteractionRequest;
using CompuStore.Store.Confirmations;

namespace CompuStore.Store.ViewModels
{
    public class ItemsMainViewModel : BindableBase
    {
        #region Fields
        private IItemService _itemService;
        private ICategoryService _categoryService;
        private string _searchText;
        private Item _selectedItem;
        private ObservableCollection<Item> _items;
        private Category _selectedCategory;
        private ObservableCollection<Category> _categories;
        #endregion
        #region Properties
        public InteractionRequest<CategoryConfirmation> CategoryEditRequest { get; set; }
        public InteractionRequest<ItemConfirmation> ItemEditRequest { get; set; }
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
        public DelegateCommand AddItemCommand => new DelegateCommand(AddItem,()=>SelectedCategory!=null).ObservesProperty(()=>SelectedCategory);
        public DelegateCommand UpdateItemCommand => new DelegateCommand(UpdateItem, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand DeleteItemCommand => new DelegateCommand(DeleteItem, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand<KeyEventArgs> SearchCommand => new DelegateCommand<KeyEventArgs>(Search);
        public DelegateCommand RefreshCommand => new DelegateCommand(Refresh, ()=>SelectedCategory!=null).ObservesProperty(()=>SelectedCategory);
        public DelegateCommand AddCategoryCommand => new DelegateCommand(AddCategory);
        public DelegateCommand UpdateCategoryCommand => new DelegateCommand(UpdateCategory, () => SelectedCategory != null).ObservesProperty(() => SelectedCategory);
        public DelegateCommand DeleteCategoryCommand => new DelegateCommand(DeleteCategory, () => SelectedCategory != null).ObservesProperty(()=>SelectedCategory);
        #endregion
        private void AddCategory()
        {
            CategoryEditRequest.Raise(new CategoryConfirmation(), x =>
             {
                 if (x.Confirmed)
                 {
                     _categoryService.Add(x.Category);
                     Categories.Add(x.Category);
                 }
             });
        }
        private void UpdateCategory()
        {
            CategoryEditRequest.Raise(new CategoryConfirmation(SelectedCategory), x =>
             {
                 if (!x.Confirmed)
                     _categoryService.Update(x.Category);
                 else
                     DataUtils.Copy(SelectedCategory, _categoryService.Find(SelectedCategory.ID));
             });
        }
        private void DeleteCategory()
        {
            if (SelectedCategory == null)
                return;
            if(!_categoryService.IsDeletable(SelectedCategory))
            {
                Messages.Error("لا يمكن حذف هذا القسم لان به اصناف");
                return;
            }
            if(Messages.Delete(SelectedCategory.Name))
            {
                _categoryService.Delete(SelectedCategory);
                Categories.Remove(SelectedCategory);
            }
        }
        private void AddItem()
        {
            ItemEditRequest.Raise(new ItemConfirmation(new Item(), SelectedCategory), x =>
             {
                 if (x.Confirmed)
                 {
                     _itemService.Add(x.Item);
                     if (SelectedCategory.ID == x.Category.ID)
                         Items.Add(x.Item);
                 }
             });
        }
        private void UpdateItem()
        {
            ItemEditRequest.Raise(new ItemConfirmation(SelectedItem,SelectedCategory), x =>
             {
                 if (x.Confirmed)
                 {
                     _itemService.Update(SelectedItem);
                     if (x.Item.CategoryID != SelectedCategory.ID)
                         Items.Remove(x.Item);
                 }
                 else
                     DataUtils.Copy(SelectedItem, _itemService.Find(SelectedItem.ID));
             });
        }
        private void DeleteItem()
        {
            if (SelectedItem == null)
                return;
            if (!_itemService.IsDeletable(SelectedItem))
                Messages.Error("لايمكن حذف صنف له عمليات بيع او شراء الا بعد حذف المبيعات والمشتريات اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name))
                    return;
                _itemService.Delete(SelectedItem);
                Items.Remove(SelectedItem);
            }
        }
        private void Refresh()
        {
            Items = new ObservableCollection<Item>(_itemService.GetAll(SelectedCategory.ID));
        }
        private void Search(KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            var i = _itemService.GetAll(SelectedCategory.ID);
            long n;
            if (long.TryParse(SearchText, out n))
                Items = new ObservableCollection<Item>(i.Where(x => x.Serial == n));
            else
                Items = new ObservableCollection<Item>(i.Where(x => x.Name.Contains(SearchText)));
            SearchText = "";
        }
        public ItemsMainViewModel(IItemService itemService,ICategoryService categoryService)
        {
            CategoryEditRequest = new InteractionRequest<CategoryConfirmation>();
            ItemEditRequest = new InteractionRequest<ItemConfirmation>();
            _itemService = itemService;
            _categoryService = categoryService;
            Categories = new ObservableCollection<Category>(_categoryService.GetAll());
            SelectedCategory = Categories.FirstOrDefault();
        }
    }
}
