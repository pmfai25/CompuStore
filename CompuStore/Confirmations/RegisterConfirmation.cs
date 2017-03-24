using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Confirmations
{
    public class RegisterConfirmation : Confirmation
    {
        public string Challenge { get; set; }
        public string Serial { get; set; }

        public RegisterConfirmation(string challenge, string serial)
        {
            Title = "";
            Serial = serial;
            Challenge = challenge;
        }
    }
}
