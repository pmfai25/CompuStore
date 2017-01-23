using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Clients.Model
{
    public class ClientPayment
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public double Money { get; set; }
        public int ClientID { get; set; }
    }
}
