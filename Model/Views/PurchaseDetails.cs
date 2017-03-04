using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Views
{
    [Table("PurchaseDetails")]
    public class PurchaseDetails:BindableBase,IDataErrorInfo
    {
        
        private string name;
        private decimal price;
        private int qunatity;
        private int available;
        private decimal total;
        [Computed]
        public decimal Total
        {
            get { return Price * Quantity; }
            set { SetProperty(ref total, value); }
        }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        public decimal Price
        {
            get { return price; }
            set {
                SetProperty(ref price, value);
                OnPropertyChanged("Total");
                OnUpdateValues?.Invoke();
            }
        }
        public int Quantity
        {
            get { return qunatity; }
            set
            {
                SetProperty(ref qunatity, value);
                OnPropertyChanged("Total");
                OnUpdateValues?.Invoke();
            }
        }
        public int Available
        {
            get { return available; }
            set { SetProperty(ref available, value); }
        }
        public int Sold { get; set; }
        public int PurchaseID { get; set; }
        public int PurchaseItemID { get; set; }
        public int ItemID { get; set; }
        public event Action OnUpdateValues;
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
        private readonly string[] properties = { "Quantity", "Price" };
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
                case "Quantity":
                    if (Quantity <= 0)
                        error = "يجب ادخال كمية اكبر من صفر";
                    else
                        if (Quantity < Sold)
                        error = "الكمية يجب ان تكون اكبر من او تساوي الكمية المباعة " +Sold;
                    break;
                case "Price":
                    if (Price<=0)
                        error = "يجب ادخال سعر اكبر من الصفر";
                    break;
            }
            return error;
        }
        #endregion
    }
}
