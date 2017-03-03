using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Views
{
    [Table("OrderDetails")]
    public class OrderDetails:BindableBase
    {
        private string name;
        private decimal retail;
        private decimal sale;
        private int qunatity;
        private decimal discount;
        private decimal total;
        [Computed]
        public decimal Total
        {
            get { return Sale * Quantity - Discount;}
            set { SetProperty(ref total, value); }
        }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }        
        public decimal Retail
        {
            get { return retail; }
            set { SetProperty(ref retail, value); }
        }
        public decimal Sale
        {
            get { return sale; }
            set { SetProperty(ref sale, value); OnPropertyChanged("Total"); if (UpdateValues != null) UpdateValues(); }
        }
        public int Quantity
        {
            get { return qunatity; }
            set { SetProperty(ref qunatity, value); OnPropertyChanged("Total"); if (UpdateValues != null) UpdateValues(); }
        }
        public decimal Discount
        {
            get { return discount; }
            set { SetProperty(ref discount, value); OnPropertyChanged("Total"); if (UpdateValues != null) UpdateValues(); }
        }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public int PurchaseItemID { get; set; }
        public int ItemID { get; set; }
        public event Action UpdateValues;
    }
}
