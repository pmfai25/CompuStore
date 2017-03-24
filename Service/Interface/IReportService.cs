using Model.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IReportService
    {
        decimal GetCurrentMoney(DateTime to);
        List<ViewIncome> GetIncome(DateTime from, DateTime to);
        List<ViewOutcome> GetOutcome(DateTime from, DateTime to);
        List<ViewItems> GetRequiredItems();
    }
}
