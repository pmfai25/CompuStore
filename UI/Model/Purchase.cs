using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Model
{
    public class Purchase
    {
        public Purchase()
        {
            this.PurchaseItems = new HashSet<PurchaseItem>();
        }

        public int ID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int SupplierID { get; set; }

        public Supplier Supplier { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
