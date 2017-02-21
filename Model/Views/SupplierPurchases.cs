using Dapper.Contrib.Extensions;
using System;
using Prism.Mvvm;
namespace Model.Views

{
    [Table("SupplierPurchases")]
    public class SupplierPurchases:BindableBase
    {
        private string name;
        private int number;
        private DateTime date;
        private decimal total;
        private decimal paid;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        public int Number
        {
            get { return number; }
            set { SetProperty(ref number, value); }
        }        
        public DateTime Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }        
        public decimal Total
        {
            get { return total; }
            set { SetProperty(ref total, value); OnPropertyChanged("Remaining"); }
        }       
        public decimal Paid
        {
            get { return paid; }
            set { SetProperty(ref paid, value); OnPropertyChanged("Remaining"); }
        }
        [Computed]
        public decimal Remaining { get { return Total - Paid; } }
        public int PurchaseID { get; set; }
        public int SupplierID { get; set; }
    }
}
