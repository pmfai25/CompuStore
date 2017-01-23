using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Purchases.Model
{
    public class Purchase
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int SupplierID { get; set; }
    }
}
