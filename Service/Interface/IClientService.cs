using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IClientService
    {
        bool Add(Client client);
        bool Update(Client client);
        bool Delete(Client client);
        bool IsDeletable(Client client);
        IEnumerable<Client> SearchBy(string name);
        IEnumerable<Client> GetAll(bool simple=false);
        Client Find(int id);
        List<Orders> GetOrders(Client client);
        List<Orders> GetOrders(Client client, DateTime dateFrom, DateTime dateTo);
    }
}
