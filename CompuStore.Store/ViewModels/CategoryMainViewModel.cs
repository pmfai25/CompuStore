using Model;
using Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using CompuStore.Infrastructure;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Store.Notification;

namespace CompuStore.Store.ViewModels
{
    public class CategoryMainViewModel : BindableBase, INavigationAware
    {
        private NavigationContext _navigationContext;
        private ICategoryService _categoryService;
        private Category _selectedItem;
        private ObservableCollection<Category> _items;
        private List<string> products;
        public List<string> Products
        {
            get { return products; }
            set { SetProperty(ref products, value); }
        }
        public ObservableCollection<Category> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }        
        public Category SelectedItem
        {
            get { return _selectedItem; }
            set {
                if (value!=null)
                    Products = _categoryService.GetNamesOfItemsForCategory(value);
                SetProperty(ref _selectedItem, value);
            }
        }
        public InteractionRequest<CategoryEditNotification> CategoryEditInteraction { get; set; }
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () =>Products!=null&& Products.Count==0).ObservesProperty(() => Products);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedItem != null).ObservesProperty(() => SelectedItem);
        public DelegateCommand BackCommand => new DelegateCommand(Back);

        private void Back()
        {
            if (_navigationContext.NavigationService.Journal.CanGoBack)
                _navigationContext.NavigationService.Journal.GoBack();
        }

        private void Add()
        {
            CategoryEditNotification notification = new Notification.CategoryEditNotification();
            notification.Title = "اضافة نوع جديد";
            
            CategoryEditInteraction.Raise(notification, x =>
            {
                if (!x.Confirmed)
                    return;
                Category c = new Category() { Name = x.Name };
                if (_categoryService.Add(c))
                    Items.Add(c);
                else
                    Messages.ErrorDataNotSaved();
            });
        }
        private void Update()
        {
            CategoryEditNotification notification = new Notification.CategoryEditNotification();
            notification.Title = "تعديل النوع";
            notification.Name = SelectedItem.Name;
            CategoryEditInteraction.Raise(notification, x =>
            {
                if (!x.Confirmed)
                    return;
                SelectedItem.Name = x.Name;
                if (!_categoryService.Update(SelectedItem))
                    Messages.ErrorDataNotSaved();
            });
        }
        private void Delete()
        {
            if (Messages.Delete(SelectedItem.Name))
            {
                _categoryService.Delete(SelectedItem);
                Items.Remove(SelectedItem);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Items = (ObservableCollection<Category>)navigationContext.Parameters["Categories"];
            SelectedItem = Items.FirstOrDefault();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public CategoryMainViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            CategoryEditInteraction = new InteractionRequest<Notification.CategoryEditNotification>();
        }
    }
}
