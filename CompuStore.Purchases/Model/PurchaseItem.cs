using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Purchases.Model
{
    public class PurchaseItem
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public double Retail { get; set; }
        public double Total { get; set; }
        public int PurchaseID { get; set; }
        public long Serial { get; set; }
    }
}
