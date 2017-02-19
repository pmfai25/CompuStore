using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Model.Views
{
    [Table("ClientOrders")]
    public class ClientOrders
    {
        public int ClientID { get; set; }
        public string Name { get; set; }
        public int OrderID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Paid { get; set; }
        [Computed]
        public decimal Remaining { get { return Total - Paid; } }
    }
}
