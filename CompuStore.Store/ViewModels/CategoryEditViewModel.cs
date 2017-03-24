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

        private CategoryConfirmation _confirmation;
        public DelegateCommand CancelCommand => new DelegateCommand(() => FinishInteraction());
        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        private void Save()
        {
            if(!Category.IsValid)
            {
                Messages.ErrorValidation();
                return;
            }
            _confirmation.Confirmed = true;
            FinishInteraction();
        }

        public Action FinishInteraction
        {
            get;set;
        }

        public INotification Notification
        {
            get
            {
                return _confirmation;
            }

            set
            {
                _confirmation = value as CategoryConfirmation;
                Category = _confirmation.Category;
                OnPropertyChanged(() => Notification);
            }
        }
    }
}
