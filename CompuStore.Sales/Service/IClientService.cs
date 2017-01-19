using CompuStore.Sales.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Sales.Service
{
    public interface IClientService
    {
        ObservableCollection<Client> GetAllClients();
    }
}
