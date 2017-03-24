using System.Collections.Generic;
using Model;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Data.SQLite;

namespace Service
{
    public class AccountService : IAccountService
    {
        private IDbConnection Connection;
        public bool AddAccount(Account account)
        {
            return Connection.Insert(account) != 0;
        }

        public bool DeleteAccount(Account account)
        {
            return Connection.Delete(account);
        }

        public IEnumerable<Account> GetAll()
        {
            return Connection.GetAll<Account>();
        }
        

        public bool UpdateAccount(Account account)
        {
            return Connection.Update(account);
        }
        public AccountService(IDbConnection connection)
        {
            Connection = connection;
        }
        public AccountService()
        {
#if DEBUG
            Connection = new SQLiteConnection("Data Source=..\\..\\Inventory.s3db; Version = 3;");
#else
            Connection = new SQLiteConnection("Data Source=Inventory.s3db; Version = 3;");  
#endif
            Connection.Open();
        }
    }
}
