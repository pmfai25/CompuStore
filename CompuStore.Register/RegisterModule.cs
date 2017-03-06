using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using Prism.Unity;

namespace CompuStore.Register
{
    public class RegisterModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public RegisterModule(RegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            
        }
    }
}