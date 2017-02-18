using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Order")]
    public class Order:BindableBase
    {
        private int number;
        private DateTime date;
        private decimal paid;
        private decimal total;
               
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
        public decimal Paid
        {
            get { return paid; }
            set { SetProperty(ref paid, value); }
        }
        public decimal Total
        {
            get { return total; }
            set { SetProperty(ref total, value); }
        }
        public int ClientID { get; set; }
        public int AccountID { get; set; }
    }
}
