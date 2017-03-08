using System.Collections.Generic;
using Model;
using Dapper.Contrib.Extensions;
using System.Data;
namespace Service
{
    class AccountService : IAccountService
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
    }
}
