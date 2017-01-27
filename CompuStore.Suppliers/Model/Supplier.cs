using System;
using System.Collections.Generic;

namespace CompuStore.Suppliers.Model
{
    public class Supplier
    {
        public Supplier()
        {
            //this.Purchases = new List<Purchase>();
            this.SupplierPayments = new List<SupplierPayment>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public decimal Initial { get; set; }

       // public List<Purchase> Purchases { get; set; }
        public List<SupplierPayment> SupplierPayments { get; set; }
    }
}
