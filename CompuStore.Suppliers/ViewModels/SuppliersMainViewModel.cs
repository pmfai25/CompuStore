using CompuStore.Suppliers.Model;
using CompuStore.Suppliers.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Suppliers.ViewModels
{
    public class SuppliersMainViewModel : BindableBase
    {
        ISuppliersService Service;
        public SuppliersMainViewModel(ISuppliersService Service)
        {
            this.Service = Service;
        }
        private SuppliersDetails selectedItem;
        public SuppliersDetails SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }
        private ObservableCollection<SuppliersDetails>  items;
        public ObservableCollection<SuppliersDetails> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }
    }
}
