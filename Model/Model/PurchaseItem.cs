using Dapper.Contrib.Extensions;
using Prism.Mvvm;

namespace Model
{
    [Table("PurchaseItem")]
    public class PurchaseItem:BindableBase
    {
        private decimal price;        
        private int quantity;
        private int available;
        public int Available
        {
            get { return available; }
            set { SetProperty(ref available, value); }
        }
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
        public int PurchaseID { get; set; }
        [Computed]
        public decimal Total { get { return Price * Quantity; } }
        public PurchaseItem()
        {
            Price = 1;
            Quantity = 1;
        }
        
    }
}
