using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Views;
using System.Data;
using Dapper.Contrib.Extensions;
using Dapper;
namespace Service
{
    public class OrderService : IOrderService
    {
        private IDbConnection Connection;      
        public IEnumerable<OrderDetails> GetOrderDetails(Orders order)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("OrderID", order.ID);
            return Connection.Query<OrderDetails>("Select * from OrderDetails where OrderID=@OrderID", args);
        }        

        public bool AddOrder(Orders order)
        {
            return Connection.Insert(order) != 0;
        }
        public bool UpdateOrder(Orders order)
        {
            return Connection.Update(order);
        }
        public bool DeleteOrder(Orders order)
        {
            return Connection.Delete(order);
        }
        public bool AddOrderDetail(OrderItem orderItem)
        {
            return Connection.Insert(orderItem) != 0;
        }
        public bool UpdateOrderDetail(OrderItem orderItem)
        {
            return Connection.Update(orderItem);
        }
        public bool DeleteOrderDetail(OrderItem orderItem)
        {
            return Connection.Delete(orderItem);
        }
        public Orders FindOrder(int orderID)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", orderID);
            return Connection.QuerySingle<Orders>("Select * from Orders where ID=@ID", args);
        }

        public void AddOrderItems(List<OrderItem> lstInsert)
        {
            Connection.Insert(lstInsert);
        }

        public void UpdateOrderItems(List<OrderItem> lstUpdate)
        {
            Connection.Update(lstUpdate);
        }

        public void DeleteOrderItems(List<OrderItem> lstDelete)
        {
            Connection.Delete(lstDelete);
        }

        public IEnumerable<ClientOrders> GetOrders()
        {
            var x = Connection.Query("Select * from ClientOrders");
            return Connection.Query<ClientOrders>("Select * from ClientOrders");
        }

        public IEnumerable<ClientOrders> GetOrders(DateTime from, DateTime to)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("DateFrom", from);
            args.Add("DateTo", to);
            return Connection.Query<ClientOrders>("Select * from  ClientOrders where Date<=@DateTo and Date>=@DateFrom", args);
        }

        public OrderService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
