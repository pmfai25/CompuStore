using CompuStore.Store.Notification;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompuStore.Store.ViewModels
{
    public class CategoryEditViewModel : BindableBase, IInteractionRequestAware
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        CategoryEditNotification notification;
        public DelegateCommand SaveCommand => new DelegateCommand(Save,()=>!string.IsNullOrWhiteSpace(Name)).ObservesProperty(() => Name);

        private void Save()
        {
            if(notification!=null)
            {
                notification.Name = name;
                notification.Confirmed = true;
                FinishInteraction?.Invoke();
            }
        }

        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);

        private void Cancel()
        {
            if (notification != null)
            {
                notification.Confirmed = false;
                FinishInteraction?.Invoke();
            }
        }

        public CategoryEditViewModel()
        {

        }

        public Action FinishInteraction
        {
            get;set;
        }

        public INotification Notification
        {
            get
            {
                return notification;
            }

            set
            {
                if (value is CategoryEditNotification)
                {
                    notification = value as CategoryEditNotification;
                    Name = notification.Name;
                    OnPropertyChanged(() => Notification);
                }
            }
        }
    }
}
