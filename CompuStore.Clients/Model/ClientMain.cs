using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using Prism.Mvvm;

namespace CompuStore.Clients.Model
{
    [Table("ClientMain")]
    public class ClientMain:BindableBase
    {
        public int ID { get; set; }
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }
        private decimal initial;
        public decimal Initial
        {
            get { return initial; }
            set { SetProperty(ref initial, value); OnPropertyChanged("Remaining"); }
        }
        private decimal sales;
        public decimal Sales
        {
            get { return sales; }
            set { SetProperty(ref sales, value); OnPropertyChanged("Remaining"); }
        }
        private decimal payments;
        public decimal Payments
        {
            get { return payments; }
            set { SetProperty(ref payments, value);OnPropertyChanged("Remaining"); }
        }
        public decimal Remaining { get { return Sales + Initial - Payments; } }
    }
}
