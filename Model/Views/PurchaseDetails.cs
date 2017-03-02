using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Views
{
    [Table("PurchaseDetails")]
    public class PurchaseDetails:BindableBase
    {
        
        private string name;
        private decimal price;
        private int qunatity;
        private decimal total;
        [Computed]
        public decimal Total
        {
            get { return Price * Quantity; }
            set { SetProperty(ref total, value); }
        }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        public decimal Price
        {
            get { return price; }
            set {
                SetProperty(ref price, value);
                OnPropertyChanged("Total");
                OnPriceUpdate?.Invoke(this);

            }
        }
        public int Quantity
        {
            get { return qunatity; }
            set
            {
                SetProperty(ref qunatity, value);
                OnPropertyChanged("Total");
                OnQuantityUpdate?.Invoke(this);
            }
        }

        public int PurchaseID { get; set; }
        public int PurchaseItemID { get; set; }
        public int ItemID { get; set; }
        public event Action<PurchaseDetails> OnPriceUpdate;
        public event Action<PurchaseDetails> OnQuantityUpdate;
    }
}
