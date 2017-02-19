using CompuStore.Infrastructure;
using Model;
using Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using Model.Events;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierEditViewModel : BindableBase,INavigationAware
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
        #endregion
        public SupplierEditViewModel(IEventAggregator eventAggregator, ISupplierService supplierService)
        {
            _eventAggregator = eventAggregator;
            _supplierService = supplierService;
        }
        private void Save()
        {
            bool _saved;
            if (CanSave())
            {
                if (_edit)
                {
                    _saved = _supplierService.Update(Supplier);
                    if(_saved)
                        _eventAggregator.GetEvent<SupplierUpdated>().Publish(Supplier);
                }
                else
                {
                    _saved = _supplierService.Add(Supplier);
                    if(_saved)
                        _eventAggregator.GetEvent<SupplierAdded>().Publish(Supplier);
                }
                if (!_saved)
                {
                    Messages.ErrorDataNotSaved();
                    return;
                }                               
                _navigationContext.NavigationService.Journal.GoBack();
            }
            else
                Messages.Error("يجب ادخال اسم ورقم تليفون للعميل");
        }
        private bool CanSave()
        {
            return !(String.IsNullOrWhiteSpace(Supplier.Name) || String.IsNullOrWhiteSpace(Supplier.Phone));
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var s2 = (Supplier)(navigationContext.Parameters["Supplier"]);
            if (s2 == null)
                return false;
            return s2.ID == Supplier.ID;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Supplier = (Supplier)(navigationContext.Parameters["Supplier"]) ?? new Supplier();
            _edit = Supplier.ID != 0;
        }
    }
}
