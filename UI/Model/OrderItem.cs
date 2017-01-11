using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Model
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int PurchaseItemID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public int OrderID { get; set; }

        public PurchaseItem PurchaseItem { get; set; }
        public Order Order { get; set; }
    }
}
