using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace Service
{
    public class ClientPaymentService : IClientPaymentService
    {
        private IDbConnection Connection;
        public bool Add(ClientPayment clientPayment)
        {
            return Connection.Insert(clientPayment) != 0;
        }

        public bool Delete(ClientPayment clientPayment)
        {
            return Connection.Delete(clientPayment);
        }

        public IEnumerable<ClientPayment> SearchByInterval(Client client,DateTime from, DateTime to)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("From", from);
            args.Add("To", to);
            args.Add("ClientID", client.ID);
            return Connection.Query<ClientPayment>("Select * from ClientPayment where ClientID=@ClientID and [Date] >= @From and [Date] <= @To", args);
        }

        public bool Update(ClientPayment clientPayment)
        {
            return Connection.Update(clientPayment);
        }

        public ClientPayment Find(int iD)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", iD);
            return Connection.QuerySingle<ClientPayment>("Select * from ClientPayment where ID=@ID", args);
        }

        public IEnumerable<ClientPayment> GetAll(Client client)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ClientID", client.ID);
            return Connection.Query<ClientPayment>("Select * from ClientPayment where ClientID=@ClientID", args);
        }

        public ClientPaymentService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
