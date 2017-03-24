using Model;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Store.Confirmations
{
    public class ItemConfirmation:Confirmation
    {
        public Item Item { get; set; }
        public Category Category { get; set; }
        public ItemConfirmation()
        {
            Title = "";
            Item = new Item();
        }
        public ItemConfirmation(Item item, Category category)
        {
            Title = "";
            Item = item;
            Category = category;
        }
    }
}
