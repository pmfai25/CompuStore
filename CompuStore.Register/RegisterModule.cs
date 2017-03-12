using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using CompuStore.Register.Views;
using CompuStore.Infrastructure;
using System.Threading.Tasks;
using Service;
using Prism.Events;
using Model.Events;
using Model;
using System;

namespace CompuStore.Register
{
    public class RegisterModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;
        ISettingsService _settingsService;
        Settings settings;

        public RegisterModule(RegionManager regionManager, IUnityContainer container, ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            _container = container;
            _settingsService = settingsService;
            _regionManager = regionManager;
        }

        public async void Initialize()
        {
            _container.RegisterTypeForNavigation<RegisterNow>(RegionNames.RegisterNow);
            if (await CheckRegisteration())
                return;
            NavigationParameters parameter = new NavigationParameters();
            parameter.Add("Finger", Finger.Value);
            _regionManager.RequestNavigate(RegionNames.MainContentRegion,RegionNames.RegisterNow,parameter);

        }
       async Task<bool> CheckRegisteration()
        {
            settings = _settingsService.Get();
            await Task.Delay(2000);
            if (Cryptor.Decrypt(settings.Serial) == Finger.Value)
                return true;
            //if(settings.Trials>0)
            //{
            //    settings.Trials--;
            //    _settingsService.Update(settings);
            //    return true;
            //}
            return false;
            
        }
    }
}