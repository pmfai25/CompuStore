using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Sales.Model
{
    public class Order
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int ClientID { get; set; }
    }
}
