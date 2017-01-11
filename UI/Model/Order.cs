using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Model
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int ID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int ClientID { get; set; }

        public Client Client { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
