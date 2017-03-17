using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Account")]
   public class Account:BindableBase,IDataErrorInfo
    {        
        public int ID { get; set; }
        private string username;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
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
        private readonly string[] properties = { "UserName", "Password" };
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
                case "UserName":
                    if (string.IsNullOrWhiteSpace(Username))
                        return "يجب ادخال اسم للمستخدم";
                    break;
                case "Password":
                    if (string.IsNullOrWhiteSpace(Password))
                        return "يجب ادخال كلمة مرور للمستخدم";
                    break;
            }
            return null;
        }
        #endregion

    }
}
