using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Views
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public int PatchID { get; set; }
        public int ItemID { get; set; }
        public string  Name { get; set; }
        public decimal Retail { get; set; }
        public decimal Sale { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        [Computed]
        public decimal Total { get { return Sale * Quantity - Discount; } }
    }
}
