using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
namespace CompuStore.Clients.Model
{
    [Table("ClientMain")]
    public class ClientMain
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public decimal Initial { get; set; }
        public decimal Sales { get; set; }
        public decimal Payments { get; set; }
        public decimal Remaining { get { return Sales + Initial - Payments; } }
    }
}
