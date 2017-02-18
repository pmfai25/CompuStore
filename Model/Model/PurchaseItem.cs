using Dapper.Contrib.Extensions;
using Prism.Mvvm;

namespace Model
{
    [Table("PurchaseItem")]
    public class PurchaseItem:BindableBase
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
        public int PatchID { get; set; }
        public int PurchaseID { get; set; }
        [Computed]
        public decimal Total { get { return Price * Quantity; } }
        
    }
}
