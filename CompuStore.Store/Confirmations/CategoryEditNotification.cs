using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Store.Notification
{
    public class CategoryEditNotification: Confirmation
    {
        public string Name { get; set; }

        public CategoryEditNotification():base()
        {
            Name = "";
        }
        public CategoryEditNotification(string name) : base()
        {
            Name = name;
        }
    }
}
