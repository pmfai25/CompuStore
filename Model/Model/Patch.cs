using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Patch")]
    class Patch:BindableBase
    {
        private decimal price;
        private int quantity;

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
        public int ItemID { get; set; }
        [Computed]
        public decimal Total { get { return Price * Quantity; } }
    }
}
