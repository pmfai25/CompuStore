using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Views;
using System.Data;
using Dapper;

namespace Service
{
    public class ReportService : IReportService
    {
        IDbConnection Connection;
        public List<ViewIncome> GetIncome(DateTime from, DateTime to)
        {

            DynamicParameters args = new DynamicParameters();
            args.Add("DateFrom", from);
            args.Add("DateTo", to);
            return Connection.Query<ViewIncome>("Select * from ViewIncome where Date<=@DateTo and Date >=@DateFrom", args).ToList();
        }

        public List<ViewOutcome> GetOutcome(DateTime from, DateTime to)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("DateFrom", from);
            args.Add("DateTo", to);
            return Connection.Query<ViewOutcome>("Select * from ViewOutcome where Date<=@DateTo and Date >=@DateFrom", args).ToList();
        }

        public decimal GetCurrentMoney(DateTime to)
        {
            DynamicParameters args = new DynamicParameters();
            args.Add("DateTo", to);
            var income=Connection.QuerySingle<decimal>("Select Total(i.Money)  from ViewIncome i where i.Date<=@DateTo", args);
            var outcome= Connection.QuerySingle<decimal>("Select Total(o.Money) from  ViewOutcome o where  o.Date<=@DateTo", args); ;
            return income - outcome;
        }

        public List<ViewItems> GetRequiredItems()
        {
            return Connection.Query<ViewItems>("Select * from ViewItems").AsList();
        }

        public ReportService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
