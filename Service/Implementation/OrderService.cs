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
        public IEnumerable<ClientOrders> GetClientOrders(Client client)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ClientID", client.ID);
            return Connection.Query<ClientOrders>("Select * from ClientOrders where ClientID=@ClientID", args);
        }
        public IEnumerable<ClientOrders> GetClientOrders(Client client, DateTime dateFrom, DateTime dateTo)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ClientID", client.ID);
            args.Add("DateFrom", dateFrom);
            args.Add("DateTo", dateTo);
            return Connection.Query<ClientOrders>("Select * from ClientOrders where ClientID=@ClientID and Date<=@DateTo and Date >=@DateTo", args);
        }
        public IEnumerable<ClientOrders> GetClientOrders(DateTime dateFrom, DateTime dateTo)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("DateFrom", dateFrom);
            args.Add("DateTo", dateTo);
            return Connection.Query<ClientOrders>("Select * from ClientOrders where Date<=@DateTo and Date >=@DateTo", args);
        }       
        public IEnumerable<OrderDetails> GetOrderDetails(Order order)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("OrderID", order.ID);
            return Connection.Query<OrderDetails>("Select * from OrderDetails where OrderID=@OrderID", args);
        }        

        public bool AddOrder(Order order)
        {
            return Connection.Insert(order) != 0;
        }
        public bool UpdateOrder(Order order)
        {
            return Connection.Update(order);
        }
        public bool DeleteOrder(Order order)
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
        public Order FindOrder(int orderID)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", orderID);
            return Connection.QuerySingle<Order>("Select * from Order where ID=@ID", args);
        }
        public OrderService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
