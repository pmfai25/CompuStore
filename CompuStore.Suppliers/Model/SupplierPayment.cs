using System;
using System.Collections.Generic;

namespace CompuStore.Suppliers.Model
{
    public class SupplierPayment
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public double Money { get; set; }
        public int SupplierID { get; set; }
        public int Number { get; set; }

        public Supplier Supplier { get; set; }
    }
}
