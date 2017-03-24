using Model;
using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Accounts.Confirmations
{
    public class AccountConfirmation :Confirmation
    {
        public Account Account { get; set; }
        public AccountConfirmation()
        {
            Title = "";
            Account = new Account();
        }
        public AccountConfirmation(Account account)
        {
            Title = "";
            Account = account;
        }
    }
}
