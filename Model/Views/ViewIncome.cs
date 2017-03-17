using Dapper.Contrib.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Views
{
    [Table("ViewIncome")]
    public class ViewIncome
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Money { get; set; }
        public string IncomeType { get; set; }
    }
}
