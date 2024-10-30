using System;

namespace SalesInventorySystem_WAM1.Models
{
    /// <summary>
    /// Represents a transaction in the inventory.
    /// </summary>
    public class Transaction
    {
        public DateTime Id { get; set; }

        public int ItemId { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
