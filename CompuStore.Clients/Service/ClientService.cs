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
            Connection = con;
        }
        public bool Add(Client client)
        {
            return Connection.Insert(client) != 0;
            
        }
        public bool Update(Client client)
        {
            bool x = Connection.Update(client);            
            return x;
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
            args.Add("Name", name);
            return Connection.Query<Client>("Select * from Client where Name like @Name +'%' or Phone like @Name+ '%'", args);            
        }

        public bool IsDeletable(Client client)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", client.ID);
            return Connection.QuerySingle<decimal>("Select Sales from Client where ID=@ID", args) != 0;
        }

        public Client Find(int id)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", id);
            return Connection.QuerySingle<Client>("Select * from Client where ID=@ID", args);
        }
    }
}
