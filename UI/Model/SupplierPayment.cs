using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Model
{
    public class SupplierPayment
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public double Money { get; set; }
        public int SupplierID { get; set; }
        public int Number { get; set; }

        public Supplier Supplier { get; set; }
    }
}
