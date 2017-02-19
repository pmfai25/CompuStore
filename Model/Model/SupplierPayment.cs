using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using Prism.Mvvm;

namespace Model
{
    [Table("SupplierPayment")]
    public class SupplierPayment:BindableBase
    {
        private int number;
        private DateTime date;
        private decimal money;
        private string notes;
        
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
        public decimal Money
        {
            get { return money; }
            set { SetProperty(ref money, value); }
        }
        public string Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }
        public int SupplierID { get; set; }
        public SupplierPayment()
        {
            Date = DateTime.Today;
        }
    }
}
