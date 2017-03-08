using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Model
{
    [Table("Item")]
    public class Item:BindableBase,IDataErrorInfo
    {
        private string name;
        private string description;
        private int limit;
        private long serial;
        private decimal price;
        private double quantity;
        public double Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }
        public int ID { get; set; }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
        public int Limit
        {
            get { return limit; }
            set { SetProperty(ref limit, value); }
        }       
        public decimal Price
        {
            get { return price; }
            set { SetProperty(ref price, value); }
        }
        public long Serial
        {
            get { return serial; }
            set { SetProperty(ref serial, value); }
        }
        public int CategoryID { get; set; }
        [Computed]
        public List<Item> Items { get; set; }
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
        private readonly string[] properties = { "Name", "Serial", "Price", "Quantity","Limit" };

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
                case "Name":
                    if (string.IsNullOrWhiteSpace(Name))
                        error = "يجب ادخال اسم للصنف";
                    else
                        if (Items.Any(x => x.Name == Name && x.ID != ID))
                        error = "يوجد صنف اخر بنفس الاسم";
                    break;
                case "Serial":
                    if (Serial < 0)
                        error = "يجب ادخال باركود بقيمة اكبر من الصفر";
                    else
                    {
                        var t = Items.FirstOrDefault(x => x.Serial == Serial && x.ID != ID);
                        if (t != null)
                            error = "هذا الباركود مستخدم من قبل للصنف " + t.Name;
                    }
                    break;
                case "Limit":
                    if (Limit < 0)
                        error = "يجب ادخال حد طلب بصفر او قيمة موجبة";
                    break;
                case "Price":
                    if (Price <= 0)
                        error = "يجب ادخال سعر اكبر من صفر";
                    break;
            }
            return error;
        }
        #endregion
    }
}
