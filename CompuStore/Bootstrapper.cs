using Microsoft.Practices.Unity;
using Prism.Unity;
using CompuStore.Views;
using System.Windows;
using Prism.Modularity;
using CompuStore.Sales.Service;
using CompuStore.Suppliers.Views;
using CompuStore.Sales.Views;
using CompuStore.Store.Views;
using CompuStore.Reports.Views;
using CompuStore.Infrastructure;

namespace CompuStore
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            this.Container.RegisterType<IClientService, ClientService>();
            this.Container.RegisterTypeForNavigation<PurchasesMain>(RegionNames.SuppliersMain);
            this.Container.RegisterTypeForNavigation<SalesMain>(RegionNames.ClientsMain);
            this.Container.RegisterTypeForNavigation<StoresMain>(RegionNames.ItemsMain);
            this.Container.RegisterTypeForNavigation<ReportsMain>(RegionNames.ReportsMain);
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(CompuStore.Suppliers.PurchasesModule));
            moduleCatalog.AddModule(typeof(CompuStore.Sales.ClientsModule));
            moduleCatalog.AddModule(typeof(CompuStore.Store.StoresModule));
            moduleCatalog.AddModule(typeof(CompuStore.Reports.ReportsModule));
        }
    }
}
