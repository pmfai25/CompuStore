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
        IEnumerable<ClientOrders> GetClientOrders(DateTime dateFrom, DateTime dateTo);
        IEnumerable<ClientOrders> GetClientOrders(Client client, DateTime dateFrom, DateTime dateTo);
        IEnumerable<OrderDetails> GetOrderDetails(ClientOrders order);
        bool AddOrder(Order order);
        bool UpdateOrder(Order order);
        bool DeleteOrder(Order order);
        bool AddOrderDetail(OrderItem orderItem);
        bool UpdateOrderDetail(OrderItem orderItem);
        bool DeleteOrderDetail(OrderItem orderItem);
    }
}
