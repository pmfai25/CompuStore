using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System.ComponentModel;
using System.Collections;

namespace Model
{
    [Table("Supplier")]
    public class Supplier:BindableBase,IDataErrorInfo
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
        private readonly string[] properties = { "Name", "Phone" };
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
            switch(property)
            {
                case "Name":
                    if (string.IsNullOrWhiteSpace(Name))
                        error= "يجب ادخال اسم للمورد";
                    break;
                case "Phone":
                    if (string.IsNullOrWhiteSpace(Phone))
                        error = "يجب ادخال رقم تليفون للمورد";
                    else
                    {
                        Phone.Trim();
                        foreach (char c in Phone)
                            if (!char.IsDigit(c))
                            {
                                error = "رقم التليفون يجب ان يحتوي على ارقام ففط";
                                break;
                            }
                    }
                    break;
            }
            return error;
        }
        #endregion
    }
}
