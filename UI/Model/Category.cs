using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Model
{
    public class Category
    {
        public Category()
        {
            this.Items = new HashSet<Item>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
