using Prism.Mvvm;

namespace CompuStore.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "CompuStore Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
