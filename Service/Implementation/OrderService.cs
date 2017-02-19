﻿using System;
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

        public IEnumerable<OrderDetails> GetOrderDetails(ClientOrders order)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("OrderID", order.OrderID);
            return Connection.Query<OrderDetails>("Select * from OrderDetails where OrderID=@OrderID", args);
        }

        public IEnumerable<ClientOrders> GetClientOrders(Client client, DateTime dateFrom, DateTime dateTo)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ClientID", client.ID);
            args.Add("DateFrom", dateFrom);
            args.Add("DateTo", dateTo);
            return Connection.Query<ClientOrders>("Select * from ClientOrders where ClientID=@ClientID and Date<=@DateTo and Date >=@DateTo", args);
        }

        public OrderService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}