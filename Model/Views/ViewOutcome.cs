using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Views
{
    [Table("ViewOutcome")]
    public class ViewOutcome
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Money { get; set; }
        public string OutcomeType { get; set; }
    }
}
