using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Model
{
    public class Supplier
    {
        public Supplier()
        {
            this.Purchases = new HashSet<Purchase>();
            this.SupplierPayments = new HashSet<SupplierPayment>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<SupplierPayment> SupplierPayments { get; set; }
    }
}
