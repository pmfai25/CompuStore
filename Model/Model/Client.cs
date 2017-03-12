using Prism.Mvvm;
using Dapper.Contrib.Extensions;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    [Table("Client")]
    public class Client:BindableBase,IDataErrorInfo
    {
        private string name;
        private string phone;
        private string address;
        private string notes;
        private decimal instantPayments;
        private decimal delayedPayments;
        private decimal sales;

      
        public int ID
        {
            get;set;
        }        
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value);
            }
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
        public decimal Sales
        {
            get { return sales; }
            set { SetProperty(ref sales, value); OnPropertyChanged("Remaining"); }
        }
        [Computed]
        public decimal Remaining { get { return Sales - DelayedPayments - InstantPayments; } }
        [Computed]
        public List<Client> Clients { get; set; }
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
                    if (string.IsNullOrWhiteSpace(Name))
                        return"يجب ادخال اسم للعميل";
                    break;
                case "Phone":
                    
                    var y = Clients.SingleOrDefault(x => x.Phone == Phone && x.ID != ID);
                    if(y!=null)
                        return "هذا الرقم مسجل من قبل مع العميل " + y.Name;
                    if (string.IsNullOrWhiteSpace(Phone))
                        return "يجب ادخال رقم تليفون للعميل";
                    foreach (char c in Phone)
                        if (!char.IsDigit(c))
                       return "رقم التليفون يجب ان يحتوي على ارقام ففط";
                    break;
            }
            return null;
        }
        #endregion
        public Client()
        {
            Clients = new List<Client>();
        }
    }
}
