using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Store.Model
{
    public class Item:BindableBase
    {
        private string name;
        private string description;
        private int limit;
        private long serial;
        private int quantity;
        private decimal salePrice;
        
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        public int Limit
        {
            get { return limit; }
            set { SetProperty(ref limit, value); }
        }
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
        public decimal SalePrice
        {
            get { return salePrice; }
            set { SetProperty(ref salePrice, value); }
        }
        public long Serial
        {
            get { return serial; }
            set { SetProperty(ref serial, value); }
        }
        [Computed]
        public int Total { get; set; }

    }
}
