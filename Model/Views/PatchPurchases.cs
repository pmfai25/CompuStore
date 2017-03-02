using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
namespace Model.Views
{
    [Table("PatchPurchases")]
    public class PatchPurchases
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CurrentQuantity { get; set; }
        public int ItemID { get; set; }
        public int PurchaseID { get; set; }
    }
}
