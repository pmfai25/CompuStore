using CompuStore.Infrastructure;
using Model.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Register.ViewModels
{
    public class RegisterNowViewModel : BindableBase, IConfirmNavigationRequest
    {
        private string serial;
        bool result;
        private string color;
        private NavigationContext _navigationContext;
        private string finger;
        private IEventAggregator _eventAggregator;

        public string Finger
        {
            get { return finger; }
            set { SetProperty(ref finger, value); }
        }
        public string Color
        {
            get { return color; }
            set { SetProperty(ref color, value); }
        }
        public string Serial
        {
            get { return serial; }
            set { SetProperty(ref serial, value); }
        }

        public DelegateCommand RegisterCommand => new DelegateCommand(Register, () => !string.IsNullOrWhiteSpace(Serial)).ObservesProperty(() => Serial);

        private void Register()
        {
            result=RegisterManager.IsValidSerial(Serial);
            if (result)
            {
                Color = "C";
                _eventAggregator.GetEvent<SerialValid>().Publish(Serial);
                Messages.Notification("تم التسجيل بنجاح");
                _navigationContext.NavigationService.Journal.GoBack();
            }
            else
                Color = "E";
        }

        void IConfirmNavigationRequest.ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(result);
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationContext = navigationContext;
            Finger = (string)navigationContext.Parameters["Finger"];
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public RegisterNowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
    }
}
