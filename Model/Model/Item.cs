using Dapper.Contrib.Extensions;
using Prism.Mvvm;
namespace CompuStore.Store.Model
{
    [Table("Item")]
    public class Item:BindableBase
    {
        private string name;
        private string description;
        private int limit;
        private long serial;
        private int quantity;
        private decimal price;
        
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
    }
}
