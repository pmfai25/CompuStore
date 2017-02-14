using CompuStore.Store.Model;
using CompuStore.Store.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Store.ViewModels
{
    public class CategoryEditViewModel : BindableBase, INavigationAware
    {
        private NavigationContext _navigationContext;
        private ICategoryService _categoryService;
        private Category _selectedCategory;
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }        
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetProperty(ref _selectedCategory, value); }
        }
        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand DeleteCommand => new DelegateCommand(Delete, () => SelectedCategory != null).ObservesProperty(() => SelectedCategory);
        public DelegateCommand UpdateCommand => new DelegateCommand(Update, () => SelectedCategory != null).ObservesProperty(() => SelectedCategory);
        private void Add()
        {
            
        }
        private void Update()
        {
            
        }
        private void Delete()
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Categories = (ObservableCollection<Category>)navigationContext.Parameters["Categories"];
            SelectedCategory = Categories.FirstOrDefault();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public CategoryEditViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
    }
}
