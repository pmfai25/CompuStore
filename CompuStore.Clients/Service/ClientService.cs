using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompuStore.Clients.Model;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;

namespace CompuStore.Clients.Service
{
    class ClientService : IClientService
    {
        SqlConnection Connection;
        public ClientService(SqlConnection con)
        {
            this.Connection = con;
        }
        public bool Add(Client client)
        {            
            long x = Connection.Insert(client);  
            return x != 0;
        }
        public bool Update(Client client)
        {
            bool x = Connection.Update(client);            
            return x;
        }
        public bool Delete(int id)
        {            
            bool x = Connection.Delete(new Client() { ID = id });            
            return x;
        }

        public IEnumerable<ClientsDetails> GetAll()
        {
            IEnumerable<ClientsDetails> x = Connection.GetAll<ClientsDetails>();            
            return x;
        }

        public IEnumerable<ClientsDetails> SearchBy(string name)
        {            
            IEnumerable<ClientsDetails> x = Connection.Query<ClientsDetails>("Select * from SuppliersDetails where Name like @Name +'%' or Phone like @Name+ '%'", name);            
            return x;
        }       
    }
}
