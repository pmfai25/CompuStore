using Model;
using Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IOrderService
    {
        IEnumerable<ClientOrders> GetClientOrders(Client client);
        IEnumerable<OrderDetails> GetOrderDetails(ClientOrders order);
        IEnumerable<ClientOrders> GetClientOrders(Client client, DateTime dateFrom, DateTime dateTo);
    }
}
