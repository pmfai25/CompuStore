using Model;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Store.Confirmations
{
    public class CategoryConfirmation : Confirmation
    {
        public Category Category { get; set; }
        public string Name { get; set; }

        public CategoryConfirmation()
        {
            Category = new Category();
            Title = "";
        }
        public CategoryConfirmation(Category category)
        {
            Category = category;
            Title = "";
        }
    }
}
