using Microsoft.Practices.Unity;
using Prism.Unity;
using CompuStore.Views;
using System.Windows;
using Prism.Modularity;

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
            this.Container.RegisterTypeForNavigation<CompuStore.Suppliers.Views.SuppliersMain>(Infrastructure.RegionNames.SuppliersMain);
            this.Container.RegisterTypeForNavigation<CompuStore.Clients.Views.ClientsMain>(Infrastructure.RegionNames.ClientsMain);
            this.Container.RegisterTypeForNavigation<CompuStore.Items.Views.ItemsMain>(Infrastructure.RegionNames.ItemsMain);
            this.Container.RegisterTypeForNavigation<CompuStore.Reports.Views.ReportsMain>(Infrastructure.RegionNames.ReportsMain);
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
