using Model;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Confirmations
{
    public class LoginConfirmation: Confirmation
    {
        public Account SelectedAccount { get; set; }
        public LoginConfirmation()
        {
            Title = "";            
        }
    }
}
