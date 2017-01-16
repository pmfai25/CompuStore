using Microsoft.Practices.Unity;
using Prism.Unity;
using CompuStore.Views;
using System.Windows;
using Prism.Modularity;
using CompuStore.Clients.Service;
using CompuStore.Suppliers.Views;
using CompuStore.Clients.Views;
using CompuStore.Items.Views;
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
            this.Container.RegisterTypeForNavigation<SuppliersMain>(RegionNames.SuppliersMain);
            this.Container.RegisterTypeForNavigation<ClientsMain>(RegionNames.ClientsMain);
            this.Container.RegisterTypeForNavigation<ItemsMain>(RegionNames.ItemsMain);
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
            moduleCatalog.AddModule(typeof(CompuStore.Suppliers.SuppliersModule));
            moduleCatalog.AddModule(typeof(CompuStore.Clients.ClientsModule));
            moduleCatalog.AddModule(typeof(CompuStore.Items.ItemsModule));
            moduleCatalog.AddModule(typeof(CompuStore.Reports.ReportsModule));
        }
    }
}
