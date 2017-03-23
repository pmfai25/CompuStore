using CompuStore.Infrastructure;
using CompuStore.Store.Confirmations;
using Model;
using Model.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Store.ViewModels
{
    public class StoreEditViewModel : BindableBase, IInteractionRequestAware
    {
        private ItemConfirmation _confirmation;
        private Item _item;
        private Category _selectedCategory;
        private ObservableCollection<Category> _categories;
        private IItemService _itemService;
        private ICategoryService _categoryService;
        public InteractionRequest<CategoryConfirmation> NewCategoryRequest { set; get; }
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
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }
        public DelegateCommand SaveCommand => new DelegateCommand(Save);
        public DelegateCommand AddCategoryCommand => new DelegateCommand(AddCategory);
        public DelegateCommand CancelCommand => new DelegateCommand(()=>FinishInteraction());
        public INotification Notification
        {
            get
            {
                return _confirmation;
            }

            set
            {
                _confirmation = (ItemConfirmation)value;
                Item = _confirmation.Item;
                Item.Items = new List<Item>(_itemService.GetAll());
                SelectedCategory = _confirmation.Category ?? Categories.First();
                OnPropertyChanged(() => this.Notification);
            }
        }
        public Action FinishInteraction
        {
            get;set;
        }
        private void AddCategory()
        {
            NewCategoryRequest.Raise(new CategoryConfirmation(), x =>
            {
                if (x.Confirmed)
                {
                    _categoryService.Add(x.Category);
                    Categories.Add(x.Category);
                    SelectedCategory = x.Category;
                }
            });
        }
        private void Save()
        {
            if(SelectedCategory==null)
            {
                Messages.Error("يجب اضافة قسم اولا ");
                return;
            }
            if (Item.IsValid)
            {
                Item.CategoryID = SelectedCategory.ID;
                _confirmation.Confirmed = true;
                FinishInteraction();
            }
            else
                Messages.ErrorValidation();
        }
        public StoreEditViewModel(ICategoryService categoryService, IItemService itemService)
        {
            NewCategoryRequest = new InteractionRequest<CategoryConfirmation>();
            _itemService = itemService;
            _categoryService = categoryService;
            Categories = new ObservableCollection<Category>(categoryService.GetAll());
        }
    }
}
