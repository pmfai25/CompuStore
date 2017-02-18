using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IClientPaymentService
    {
        bool Add(ClientPayment clientPayment);
        bool Update(ClientPayment clientPayment);
        bool Delete(ClientPayment clientPayment);
        IEnumerable<ClientPayment> SearchByInterval(Client client,DateTime from, DateTime to);
        IEnumerable<ClientPayment> GetAll(Client client);
        ClientPayment Find(int iD);
    }
}
