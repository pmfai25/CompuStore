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
