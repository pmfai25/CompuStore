using CompuStore.Infrastructure;
using Model.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using Service;
using Model;
namespace CompuStore.Register.ViewModels
{
    public class RegisterNowViewModel : BindableBase, IConfirmNavigationRequest
    {
        private string serial;
        private bool result;
        private string color;
        private ISettingsService _service;
        private NavigationContext _navigationContext;
        private string finger;

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

        public DelegateCommand RegisterCommand => new DelegateCommand(Register);

        private void Register()
        {
            result=RegisterManager.IsValidSerial(Serial);
            if (result)
            {
                Settings settings = _service.Get();
                settings.Serial = Serial;
                _service.Update(settings);
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

        public RegisterNowViewModel(ISettingsService service)
        {
            _service = service;
        }
    }
}
