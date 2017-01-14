using Microsoft.Practices.Unity;
using Prism.Unity;
using CompuStore.Views;
using System.Windows;

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
    }
}
