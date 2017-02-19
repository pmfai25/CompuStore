using Microsoft.Practices.Unity;
using Prism.Modularity;
using System.Data;
using System.Data.SQLite;
namespace Service
{
    public class ServiceModule : IModule
    {
        IUnityContainer _container;
        SQLiteConnection Connection;
        
        public ServiceModule(IUnityContainer container)
        {
            _container = container;
            
        }

        public void Initialize()
        {
            Connection = new SQLiteConnection("Data Source=Inventory.s3db;");
            Connection.Open();
            _container.RegisterInstance<IDbConnection>(Connection);
            _container.RegisterType<IClientService, ClientService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IClientPaymentService, ClientPaymentService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISupplierService, SupplierService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISupplierPaymentService, SupplierPaymentService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderService, OrderService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IItemService, ItemService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICategoryService, CategoryService>(new ContainerControlledLifetimeManager());
        }
    }
}