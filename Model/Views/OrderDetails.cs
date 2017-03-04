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
    [Table("OrderDetails")]
    public class OrderDetails:BindableBase,IDataErrorInfo
    {
        private string name;
        private decimal retail;
        private decimal price;
        private int qunatity;
        private decimal discount;
        private decimal total;
        private long serial;
        public int OldQuantity { set; get; }
        public int Available { get; set; }
        [Computed]
        public decimal Total
        {
            get { return Price * Quantity - Discount;}
            set { SetProperty(ref total, value); }
        }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        public long Serial
        {
            get { return serial; }
            set { SetProperty(ref serial, value); }
        }
        public decimal Retail
        {
            get { return retail; }
            set { SetProperty(ref retail, value); }
        }
        public decimal Price
        {
            get { return price; }
            set
            {
                SetProperty(ref price, value);
                OnPropertyChanged("Total");
                UpdateValues?.Invoke();
            }
        }
        public int Quantity
        {
            get { return qunatity; }
            set
            {
                SetProperty(ref qunatity, value);
                OnPropertyChanged("Total");
                UpdateValues?.Invoke();
            }
        }
        public decimal Discount
        {
            get { return discount; }
            set
            {
                SetProperty(ref discount, value);
                OnPropertyChanged("Total");
                UpdateValues?.Invoke();
            }
        }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public int PurchaseItemID { get; set; }
        public int ItemID { get; set; }
        public event Action UpdateValues;
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
        private readonly string[] properties = { "Quantity", "Price", "Discount" };
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
                        if (OrderItemID==0&& Quantity > Available)
                        error = "الكمية يجب ان تكون اقل من او تساوي المتاح   " + Available;
                    else
                        if(OrderItemID!=0 &&Quantity>OldQuantity+Available )
                        error = "الكمية المتاحة الان هي " + Available + Environment.NewLine + "يمكن تعديل الكمية بحيث لا تتعدى " + (OldQuantity + Available);
                    break;
                case "Price":
                    if (Price <= 0)
                        error = "يجب ادخال سعر اكبر من الصفر";
                    break;
                case "Discount":
                    if (Discount < 0)
                        error = "الخصم لا يكون بالسالب";
                    break;
            }
            return error;
        }
        #endregion
    }
}
