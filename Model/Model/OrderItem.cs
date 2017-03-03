using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("OrderItem")]
    public class OrderItem:BindableBase
    {
        private decimal price;
        private int quantity;
        private decimal discount;

        public int ID { get; set; }
        public decimal Price
        {
            get { return price; }
            set { SetProperty(ref price, value); OnPropertyChanged("Total"); }
        }
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); OnPropertyChanged("Total"); }
        }
        public decimal Discount
        {
            get { return discount; }
            set { SetProperty(ref discount, value); OnPropertyChanged("Total"); }
        }
        public int PurchaseItemID { get; set; }
        public int OrderID { get; set; }
        [Computed]
        public decimal Total { get { return Price * Quantity - Discount; } }
    }
}
