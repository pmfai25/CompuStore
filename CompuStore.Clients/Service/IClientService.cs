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
        bool Add(Client supplier);
        bool Update(Client supplier);
        bool Delete(int ID);
        IEnumerable<ClientsDetails> SearchBy(string name);
        IEnumerable<ClientsDetails> GetAll();
    }
}
