using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;

namespace Model
{
    [Table("Purchase")]
    public class Purchase:BindableBase,IDataErrorInfo
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
        
        #region IDataErrorInfo
        string IDataErrorInfo.Error
        {
            get
            {
                return null;
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return GetValidationError(propertyName);
            }
        }
        #endregion
        #region Validation
        private readonly string[] properties = { "Paid","Total" };
        [Computed]
        public bool IsValid
        {
            get
            {
                foreach (var s in properties)
                    if (GetValidationError(s) != null)
                        return false;
                return true;
            }
        }

        private string GetValidationError(string property)
        {
            string error = null;
            switch (property)
            {
                case "Paid":
                    if (Paid < 0)
                        error = " المدفوع يجب ان يكون اكبر من او يساوي صفر";
                    else 
                    if (Remaining < 0)
                        error = "المدفوع يجب ان يكون اقل من الاجمالي";
                    break;
                case "Total":
                    if (Total == 0)
                        error = "يجب اضافة اصناف للفاتورة";
                    break;              
            }
            return error;
        }
        #endregion
        public Purchase()
        {
            Date = DateTime.Today;
        }
        public Purchase(int supplierID)
        {
            SupplierID = supplierID;
            Date = DateTime.Today;
        }
    }
}
