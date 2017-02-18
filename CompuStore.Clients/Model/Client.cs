using Prism.Mvvm;
using Dapper.Contrib.Extensions;
namespace CompuStore.Clients.Model
{
    [Table("Client")]
    public class Client:BindableBase
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
    }
}
