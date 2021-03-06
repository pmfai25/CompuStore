using System.Windows;
using CompuStore.Clients;
using CompuStore.Infrastructure;
using CompuStore.Reports;
using CompuStore.Reports.Views;
using CompuStore.Store;
using CompuStore.Suppliers;
using CompuStore.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using System.Threading;
using Service;
using System.Globalization;
using System;
using CompuStore.Register;
using Prism.Regions;
using Prism.Events;
using Model.Events;
using System.Threading.Tasks;
using CompuStore.Accounts;

namespace CompuStore
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture("ar-eg");
            ci.DateTimeFormat.ShortDatePattern = "dd / MM / yyyy";
            ci.DateTimeFormat.FullDateTimePattern= "dd / MM / yyyy";
            ci.NumberFormat = CultureInfo.GetCultureInfo("en-us").NumberFormat;
            Thread.CurrentThread.CurrentCulture = ci;
            return Container.Resolve<Shell>();
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }        
        protected override void InitializeShell()
        {
            try
            {
                Application.Current.MainWindow.Show();
            }
            catch (Exception ex)
            {
                Messages.Error("حدث خطا بالبرنامج" + Environment.NewLine + ex.Message);
            }

        }
        protected override void ConfigureModuleCatalog()
        {            
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(ServiceModule));
            moduleCatalog.AddModule(typeof(AccountsModule));
            moduleCatalog.AddModule(typeof(StoreModule));
            moduleCatalog.AddModule(typeof(ClientsModule));
            moduleCatalog.AddModule(typeof(SuppliersModule));
            moduleCatalog.AddModule(typeof(ReportsModule));              
        }
        public override async void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);
            IEventAggregator eventAggregator = Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<DoLogin>().Publish();

            await Task.Delay(2000);
            ISettingsService settingsService = Container.Resolve<ISettingsService>();
            string serial = settingsService.Get().Serial;
            string correctSerial = Cryptor.Encrypt(Finger.Value);
            if (serial!=correctSerial)
                eventAggregator.GetEvent<DoRegister>().Publish(new RegisterValues() { Challenge = Finger.Value, Serial = correctSerial });
        }
    }
}
