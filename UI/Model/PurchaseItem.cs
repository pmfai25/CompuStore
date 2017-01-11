using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Model
{
    public class PurchaseItem
    {
        public PurchaseItem()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int ID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public double Retail { get; set; }
        public double Total { get; set; }
        public int PurchaseID { get; set; }
        public long Serial { get; set; }

        public Item Item { get; set; }
        public Purchase Purchase { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
