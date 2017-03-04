using System.Collections.Generic;
using Model;
using System.Data.SqlClient;
using Dapper.Contrib.Extensions;
using Dapper;
using System.Data;
using System;

namespace Service{
    public class ClientService : IClientService
    {
        IDbConnection Connection;
        public ClientService(IDbConnection con)
        {
            Connection = con;
        }
        public bool Add(Client client)
        {
            return Connection.Insert(client) != 0;
            
        }
        public bool Update(Client client)
        {
           return Connection.Update(client);            
        }
        public bool Delete(Client client)
        {            
            return Connection.Delete(client);            
        }
        public IEnumerable<Client> GetAll()
        {
            return  Connection.GetAll<Client>();            
        }

        public IEnumerable<Client> SearchBy(string name)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("Name", name+"%");
            return Connection.Query<Client>("Select * from Client where Name like @Name or Phone like @Name", args);            
        }
        public Client Find(int id)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", id);
            return Connection.QuerySingle<Client>("Select * from Client where ID=@ID", args);
        }
        public bool IsDeletable(Client client)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", client.ID);
            return Connection.QuerySingle<decimal>("Select Sales from Client where ID=@ID", args) != 0;
        }

        public List<Orders> GetOrders(Client client)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ClientID", client.ID);
            return new List<Orders>(Connection.Query<Orders>("Select * from Orders where ClientID=@ClientID", args));

        }
        public List<Orders> GetOrders(Client client, DateTime dateFrom, DateTime dateTo)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ClientID", client.ID);
            args.Add("DateFrom", dateFrom);
            args.Add("DateTo", dateTo);
            return new List<Orders>(Connection.Query<Orders>("Select * from Orders where ClientID=@ClientID and Date <=@DateTo and Date>=@DateFrom", args));
        }
    }
}
