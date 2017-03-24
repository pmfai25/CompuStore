using Model.Views;
using Prism.Mvvm;
using Service;
using System.Collections.Generic;

namespace CompuStore.Reports.ViewModels
{
    public class RequiredItemsReportViewModel : BindableBase
    {

        private List<ViewItems> _items;
        private IReportService _reportService;

        public List<ViewItems> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public RequiredItemsReportViewModel(IReportService reportService)
        {
            _reportService = reportService;
            Items = _reportService.GetRequiredItems();
        }
    }
}
