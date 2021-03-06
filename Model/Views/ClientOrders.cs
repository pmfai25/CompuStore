using System;
using Dapper.Contrib.Extensions;
using Prism.Mvvm;

namespace Model.Views
{
    [Table("ClientOrders")]
    public class ClientOrders : BindableBase
    {

        private string name;
        private int number;
        private DateTime date;
        private decimal total;
        private decimal paid;
        private decimal remaining;
        private decimal finaltProfit;        
        private decimal currentProfit;

        public decimal CurrentProfit
        {
            get { return currentProfit; }
            set { SetProperty(ref currentProfit, value); }
        }
        public decimal FinalProfit
        {
            get { return finaltProfit; }
            set { SetProperty(ref finaltProfit, value); }
        }
        public decimal Remaining
        {
            get { return Total - Paid; }
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

        public int ClientID { get; set; }
        public int OrderID { get; set; }
    }
}
