using Microsoft.Practices.Unity;
using Prism.Unity;
using CompuStore.Views;
using System.Windows;
using Prism.Modularity;
using CompuStore.Suppliers.Views;
using CompuStore.Sales.Views;
using CompuStore.Store.Views;
using CompuStore.Reports.Views;
using CompuStore.Infrastructure;
using CompuStore.Clients.Views;
using CompuStore.Purchases.Views;
using Prism.Regions;

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
            //Register Services

            //Register Main Views
            this.Container.RegisterTypeForNavigation<PurchasesMain>(RegionNames.PurchasesMain);
            this.Container.RegisterTypeForNavigation<SalesMain>(RegionNames.SalesMain);
            this.Container.RegisterTypeForNavigation<StoreMain>(RegionNames.StoreMain);
            this.Container.RegisterTypeForNavigation<ClientsMain>(RegionNames.ClientsMain);
            this.Container.RegisterTypeForNavigation<SuppliersMain>(RegionNames.SuppliersMain);
            this.Container.RegisterTypeForNavigation<ReportsMain>(RegionNames.ReportsMain);
            //Register Editing Views
        }
        
        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(CompuStore.Sales.SalesModule));
            moduleCatalog.AddModule(typeof(CompuStore.Purchases.PurchasesModule));
            moduleCatalog.AddModule(typeof(CompuStore.Store.StoreModule));
            moduleCatalog.AddModule(typeof(CompuStore.Clients.ClientsModule));
            moduleCatalog.AddModule(typeof(CompuStore.Suppliers.SuppliersModule));
            moduleCatalog.AddModule(typeof(CompuStore.Reports.ReportsModule));
        }
    }
}
