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
        IEnumerable<OrderDetails> GetOrderDetails(Orders order);
        bool AddOrder(Orders order);
        bool UpdateOrder(Orders order);
        bool DeleteOrder(Orders order);
        bool AddOrderDetail(OrderItem orderItem);
        bool UpdateOrderDetail(OrderItem orderItem);
        bool DeleteOrderDetail(OrderItem orderItem);
        Orders FindOrder(int orderID);
        void AddOrderItems(List<OrderItem> lstInsert);
        void UpdateOrderItems(List<OrderItem> lstUpdate);
        void DeleteOrderItems(List<OrderItem> lstDelete);
    }
}
