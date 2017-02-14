using System.Windows;
using CompuStore.Clients;
using CompuStore.Infrastructure;
using CompuStore.Purchases;
using CompuStore.Purchases.Views;
using CompuStore.Reports;
using CompuStore.Reports.Views;
using CompuStore.Sales;
using CompuStore.Sales.Views;
using CompuStore.Store;
using CompuStore.Store.Views;
using CompuStore.Suppliers;
using CompuStore.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;

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
            Manager.Connection.Open();
            Container.RegisterInstance(Manager.Connection);
            Container.RegisterTypeForNavigation<PurchasesMain>(RegionNames.PurchasesMain);
            Container.RegisterTypeForNavigation<SalesMain>(RegionNames.SalesMain);
            Container.RegisterTypeForNavigation<StoreMain>(RegionNames.StoreMain);            
            Container.RegisterTypeForNavigation<ReportsMain>(RegionNames.ReportsMain);
        }

        
        protected override void InitializeShell()
        {
            
            Application.Current.MainWindow.Show();
        }
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(SalesModule));
            moduleCatalog.AddModule(typeof(PurchasesModule));
            moduleCatalog.AddModule(typeof(StoreModule));
            moduleCatalog.AddModule(typeof(ClientsModule));
            moduleCatalog.AddModule(typeof(SuppliersModule));
            moduleCatalog.AddModule(typeof(ReportsModule));
        }
    }
}
