using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Model
{
    public class Client
    {
        public Client()
        {
            this.ClientPayments = new HashSet<ClientPayment>();
            this.Orders = new HashSet<Order>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }

        public ICollection<ClientPayment> ClientPayments { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
