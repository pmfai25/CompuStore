using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Model
{
    public class Item
    {
        public Item()
        {
            this.PurchaseItems = new HashSet<PurchaseItem>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public int Limit { get; set; }
        public string Description { get; set; }

        public Category Category { get; set; }
        public ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
