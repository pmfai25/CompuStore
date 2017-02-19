using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using Prism.Mvvm;

namespace Model
{
    [Table("Supplier")]
    public class Supplier:BindableBase
    {

        private string name;
        private string phone;
        private string address;
        private string notes;
        private decimal purchase;
        private decimal instantPayments;
        private decimal delayedPayments;

        public int ID
        {
            get;set;
        }       
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }        
        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }      
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }        
        public string Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }        
        public decimal Purchases
        {
            get { return purchase; }
            set { SetProperty(ref purchase, value); OnPropertyChanged("Remaining"); }
        }
        public decimal InstantPayments
        {
            get { return instantPayments; }
            set { SetProperty(ref instantPayments, value); OnPropertyChanged("Remaining"); }
        }
        public decimal DelayedPayments
        {
            get { return delayedPayments; }
            set { SetProperty(ref delayedPayments, value); OnPropertyChanged("Remaining"); }
        }
        [Computed]
        public decimal Remaining { get { return Purchases - InstantPayments - DelayedPayments; } }
    }
}
