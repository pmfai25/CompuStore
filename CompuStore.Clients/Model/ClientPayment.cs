using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
namespace CompuStore.Clients.Model
{
    [Table("ClientPayment")]
    public class ClientPayment
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Money { get; set; }
        public int ClientID { get; set; }
    }
}
