using CompuStore.Infrastructure;
using CompuStore.Suppliers.Confirmations;
using Model;
using Model.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Service;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompuStore.Suppliers.ViewModels
{
    public class SupplierNavigationViewModel : BindableBase
    {
        #region Fields
        private Supplier _selectedItem;
        private string _searchText;
        private ObservableCollection<Supplier> _items;
        private readonly ISupplierService _supplierService;
        private IEventAggregator _eventAggregator;
        #endregion
        #region Properties
        public InteractionRequest<SupplierConfirmation> SupplierEditRequest { get; set; }
        public Supplier SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                bool skip = false;
                if ((SelectedItem != null && value != null && SelectedItem.ID == value.ID))
                    skip = true;
                SetProperty(ref _selectedItem, value);
                if(!skip)
                    _eventAggregator.GetEvent<SupplierSelected>().Publish(SelectedItem);
            }
        }
        public ObservableCollection<Supplier> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); SelectedItem = Items.FirstOrDefault(); }
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
        public DelegateCommand SearchCommand => new DelegateCommand(Search);
        #endregion
        #region Methods
        private void Add()
        {
            SupplierEditRequest.Raise(new SupplierConfirmation(), x =>
            {
                if (x.Confirmed)
                {
                    if (_supplierService.Add(x.Supplier))
                    {
                        Items.Add(x.Supplier);
                        SelectedItem = x.Supplier;
                    }
                    else
                        Messages.Error("يوجد مورد بنفس رقم التليفون");
                }
            });
        }
        private void Update()
        {
            SupplierEditRequest.Raise(new SupplierConfirmation(SelectedItem), x =>
            {
                if(!x.Confirmed)
                {
                    DataUtils.Copy(SelectedItem, _supplierService.Find(SelectedItem.ID));
                    return;
                }
                if (!_supplierService.Update(x.Supplier))
                {
                    Messages.Error("يوجد مورد بنفس رقم التليفون");
                    DataUtils.Copy(SelectedItem, _supplierService.Find(SelectedItem.ID));
                }                   
            });
        }
        private void Delete()
        {
            if (SelectedItem == null)
                return;
            if (!_supplierService.IsDeleteable(SelectedItem))
                Messages.Error("لايمكن حذف شركة لها عمليات شراء الا بعد حذف المشتريات اولا");
            else
            {
                if (!Messages.Delete(SelectedItem.Name))
                    return;
                _supplierService.Delete(SelectedItem);
                Items.Remove(SelectedItem);
            }
        }
        private void Search()
        {
            Items = new ObservableCollection<Supplier>(_supplierService.SearchBy(SearchText));
        }
        #endregion
        public SupplierNavigationViewModel(ISupplierService supplierService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _searchText = "";
            _supplierService = supplierService;
            SupplierEditRequest = new InteractionRequest<SupplierConfirmation>();            
            Items = new ObservableCollection<Supplier>(_supplierService.GetAll());
        }
    }
}
