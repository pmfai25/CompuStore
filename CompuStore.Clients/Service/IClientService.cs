using CompuStore.Clients.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Clients.Service
{
    public interface IClientService
    {
        ObservableCollection<Client> GetAllClients();
    }
}
