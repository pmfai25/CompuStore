using System;
using System.Collections.Generic;

namespace CompuStore.Suppliers.Model
{
    public class SuppliersDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public decimal Initial { get; set; }
        public decimal? Purchases { get; set; }
        public double? Payments { get; set; }
        public long? Count { get; set; }
    }
}
