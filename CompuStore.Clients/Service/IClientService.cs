using CompuStore.Clients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Clients.Service
{
    public interface IClientService
    {
        bool Add(Client client);
        bool Update(Client client);
        bool Delete(Client client);
        bool IsClientWithOrders(int id);
        IEnumerable<ClientMain> SearchBy(string name);
        IEnumerable<ClientMain> GetAll();
        Client Find(int id);
        ClientMain FindClientMain(int iD);
    }
}
