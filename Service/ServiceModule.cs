using Microsoft.Practices.Unity;
using Prism.Modularity;
using System.Data;
using System.Data.SQLite;
using Dapper;
using Dapper.Contrib.Extensions;
using Prism.Regions;
using CompuStore.Infrastructure;

namespace Service
{
    public class ServiceModule : IModule
    {
        IUnityContainer _container;
        SQLiteConnection Connection;
        IRegionManager _regionManager;
        public ServiceModule(IUnityContainer container, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _container = container;            
        }

        public void Initialize()
        {
#if DEBUG
            Connection = new SQLiteConnection("Data Source=..\\..\\Inventory.s3db; Version = 3;");
#else
            Connection = new SQLiteConnection("Data Source=Inventory.s3db; Version = 3;");  
#endif
            Connection.Open();
            Connection.Execute("PRAGMA foreign_keys = ON");
            _container.RegisterInstance<IDbConnection>(Connection);
            _container.RegisterType<IClientService, ClientService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IClientPaymentService, ClientPaymentService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISupplierService, SupplierService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISupplierPaymentService, SupplierPaymentService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderService, OrderService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPurchaseService, PurchaseService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IItemService, ItemService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICategoryService, CategoryService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IReportService, ReportService>(new ContainerControlledLifetimeManager());
        }
    }
}