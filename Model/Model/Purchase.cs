using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;

namespace Model
{
    [Table("Purchase")]
    public class Purchase:BindableBase
    {
        private int number;        
        private DateTime date;        
        private decimal paid;        
        private decimal total;
        private decimal remaining;
        public int ID { get; set; }
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
        public decimal Remaining
        {
            get { return Total - Paid; }
            set { SetProperty(ref remaining, value); }
        }
        public int SupplierID { get; set; }
    }
}
