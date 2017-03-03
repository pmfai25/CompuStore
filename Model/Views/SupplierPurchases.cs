using Dapper.Contrib.Extensions;
using System;
using Prism.Mvvm;
using System.Collections.ObjectModel;

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
        private decimal remaining;
        private int suppliedID;
        public int SupplierID
        {
            get { return suppliedID; }
            set { SetProperty(ref suppliedID, value); }
        }
        [Computed]
        public decimal Remaining
        {
            get {  return Total - Paid;  }
            set { SetProperty(ref remaining, value); }
        }
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
        public int PurchaseID { get; set; }
        
    }
}
