using CompuStore.Confirmations;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace CompuStore.ViewModels
{
    public class RegisterViewModel : BindableBase, IInteractionRequestAware
    {
        private string serial;
        private string challenge;
        private SolidColorBrush statusBrush;
        private RegisterConfirmation confirmation;
        public SolidColorBrush StatusBrush
        {
            get { return statusBrush; }
            set { SetProperty(ref statusBrush, value); }
        }
        public string Challenge
        {
            get { return challenge; }
            set { SetProperty(ref challenge, value); }
        }
        public string Serial
        {
            get { return serial; }
            set { SetProperty(ref serial, value); }
        }
        public DelegateCommand<KeyEventArgs> CheckSerialCommand => new DelegateCommand<KeyEventArgs>(CheckSerial);

        private void CheckSerial(KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            if (Serial == confirmation.Serial)
            {
                confirmation.Confirmed = true;
                FinishInteraction();
            }
            else
                StatusBrush = Brushes.Red;
        }

        public RegisterViewModel()
        {
            Serial = "";
            Challenge = "";
            StatusBrush = Brushes.White;
        }

        public Action FinishInteraction
        {
            get;set;
        }

        public INotification Notification
        {
            get
            {
                return confirmation;
            }

            set
            {
                confirmation = (RegisterConfirmation)value;
                Challenge = confirmation.Challenge;
            }
        }
    }
}
