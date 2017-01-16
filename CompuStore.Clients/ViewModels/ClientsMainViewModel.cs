using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Regions;
using Prism.Interactivity.InteractionRequest;
using CompuStore.Clients.Properties;

namespace CompuStore.Clients.ViewModels
{
    public class ClientsMainViewModel : BindableBase, IConfirmNavigationRequest
    {
        private readonly InteractionRequest<Confirmation> confirmExitInteractionRequest;
        public IInteractionRequest ConfirmExitInteractionRequest
        {
            get { return this.confirmExitInteractionRequest; }
        }
        int x;
        public ClientsMainViewModel()
        {
            confirmExitInteractionRequest = new InteractionRequest<Confirmation>();
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            if (x==1)
            {
                this.confirmExitInteractionRequest.Raise(
                    new Confirmation { Content = Resources.ConfirmExitMessage, Title = Resources.ConfirmExitTitle },
                    c => { continuationCallback(c.Confirmed); });
            }
            else
            {
                continuationCallback(true);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            x = (int)navigationContext.Parameters["X"];
        }
    }
}
