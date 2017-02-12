using CompuStore.Clients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompuStore.Clients.Service
{
    public interface IClientPaymentService
    {
        bool Add(ClientPayment clientPayment);
        bool Update(ClientPayment clientPayment);
        bool Delete(ClientPayment clientPayment);
        IEnumerable<ClientPayment> SearchByInterval(int clientID,DateTime from, DateTime to);
        ClientPayment Find(int iD);
    }
}
