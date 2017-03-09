using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using Model.Events;
using System.Collections.Generic;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierEditViewModel : BindableBase,INavigationAware,IRegionMemberLifetime
    {
        #region Fields
        private bool _edit;
        private Supplier _supplier;
        private readonly ISupplierService _supplierService;
        private readonly IEventAggregator _eventAggregator;
        private NavigationContext _navigationContext;
        #endregion
        #region Properties
        public Supplier Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier, value); }
        }
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);
        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        public bool KeepAlive
        {
            get
            {
                return false;
            }
        }
        #endregion
        #region Methods
        private void Save()
        {
            if (!Supplier.IsValid)
            {
                Messages.Error("يوجد خطا في بعض البيانات");
                return;
            }

            if (_edit && _supplierService.Update(Supplier))
                _eventAggregator.GetEvent<SupplierUpdated>().Publish(Supplier);
            else
            if (!_edit && _supplierService.Add(Supplier))
                _eventAggregator.GetEvent<SupplierAdded>().Publish(Supplier);
            else
            {
                Messages.ErrorDataNotSaved();
                return;
            }
            _navigationContext.NavigationService.Journal.GoBack();

        }
        private void Cancel()
        {
            if (_edit)
            {
                var s2 = _supplierService.Find(Supplier.ID);
                DataUtils.Copy(Supplier, s2);
            }
            _navigationContext.NavigationService.Journal.GoBack();
        }
        #endregion
        #region Interface
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Supplier = (Supplier)(navigationContext.Parameters["Supplier"]) ?? new Supplier();
            Supplier.Suppliers = new List<Supplier>(_supplierService.GetAll(true));
            _edit = Supplier.ID != 0;
        }
        #endregion
        public SupplierEditViewModel(IEventAggregator eventAggregator, ISupplierService supplierService)
        {
            _eventAggregator = eventAggregator;
            _supplierService = supplierService;
        }
    }
}
