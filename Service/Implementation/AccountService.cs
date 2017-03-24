using System.Collections.Generic;
using Model;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Data.SQLite;
using System;
using Dapper;

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

        public Account Find(int ID)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("ID", ID);
            return Connection.QuerySingle<Account>("Select * from Account where ID=@ID", args);
        }

        public List<Orders> GetSales(Account account)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("AccountID", account.ID);
            return Connection.Query<Orders>("Select * from Orders where AccountID=@AccountID", args).AsList();
        }

        public List<Orders> GetSales(Account account, DateTime dateFrom, DateTime dateTo)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("AccountID", account.ID);
            args.Add("DateFrom", dateFrom);
            args.Add("DateTo", dateTo);
            return Connection.Query<Orders>("Select * from Orders where AccountID=@AccountID and Date<=@DateTo and Date>=@DateFrom", args).AsList();
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
