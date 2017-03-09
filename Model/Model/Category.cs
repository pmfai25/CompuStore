using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Model
{
    [Table("Category")]
    public class Category:BindableBase,IDataErrorInfo
    {
        public int ID { get; set; }
        private string name;
        public List<Category> Categories { get; set; }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
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
        private readonly string[] properties = { "Name" };
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
            switch (property)
            {
                case "Name":
                    Name = Name.Trim();
                    if (Categories.Any(x => x.Name == Name && x.ID!=ID))
                        return "يوجد قسم اخر بنفس الاسم";
                    if (string.IsNullOrWhiteSpace(Name))
                        return "يجب ادخال اسم ";
                    break;
                  
            }
            return null;
        }
        #endregion
    }
}
