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
using System.Threading;
using Service;

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
            moduleCatalog.AddModule(typeof(ServiceModule));
           // moduleCatalog.AddModule(typeof(SalesModule));
            //moduleCatalog.AddModule(typeof(PurchasesModule));
            moduleCatalog.AddModule(typeof(StoreModule));
            moduleCatalog.AddModule(typeof(ClientsModule));
            moduleCatalog.AddModule(typeof(SuppliersModule));
            moduleCatalog.AddModule(typeof(ReportsModule));
            
        }
    }
}
