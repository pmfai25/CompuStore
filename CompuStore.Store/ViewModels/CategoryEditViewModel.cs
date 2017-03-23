using CompuStore.Infrastructure;
using CompuStore.Store.Confirmations;
using Model;
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
        private Category _category;
        public Category Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }

        CategoryConfirmation notification;
        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        private void Save()
        {
            if(!Category.IsValid)
            {
                Messages.ErrorValidation();
                return;
            }
            notification.Confirmed = true;
            FinishInteraction();
        }

        public DelegateCommand CancelCommand => new DelegateCommand(()=>FinishInteraction());
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
                notification = value as CategoryConfirmation;
                Category = notification.Category;
                OnPropertyChanged(() => Notification);
            }
        }
    }
}
