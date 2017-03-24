using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAll();
        bool AddAccount(Account account);
        bool UpdateAccount(Account account);
        bool DeleteAccount(Account account);
        List<Orders> GetSales(Account account);
        List<Orders> GetSales(Account account, DateTime dateFrom, DateTime dateTo);
        Account Find(int ID);
    }
}
