using Prism.Mvvm;
using Dapper.Contrib.Extensions;
namespace CompuStore.Clients.Model
{
    [Table("Client")]
    public class Client:BindableBase
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value);
            }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        private string notes;
        public string Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }
        private decimal initial;
        public decimal Initial
        {
            get { return initial; }
            set { SetProperty(ref initial, value); }
        }
    }
}
