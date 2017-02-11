using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompuStore.Clients.Model;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using Dapper;

namespace CompuStore.Clients.Service
{
    public class ClientPaymentService : IClientPaymentService
    {
        private SqlConnection Connection;
        public bool Add(ClientPayment clientPayment)
        {
            return Connection.Insert(clientPayment) != 0;
        }

        public bool Delete(ClientPayment clientPayment)
        {
            return Connection.Delete(clientPayment);
        }

        public IEnumerable<ClientPayment> SearchByInterval(DateTime from, DateTime to)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("From", from);
            args.Add("To", to);
            return Connection.Query<ClientPayment>("Select * from ClientPayment where [Date] >= @From and [Date] <= @To", args);
        }

        public bool Update(ClientPayment clientPayment)
        {
            return Connection.Update(clientPayment);
        }
        public ClientPaymentService(SqlConnection connection)
        {
            Connection = connection;
        }
    }
}
