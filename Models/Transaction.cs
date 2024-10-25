using System;

namespace SalesInventorySystem_WAM1.Models
{
    public class Transaction
    {
        public DateTime Timestamp { get; }

        public int ItemId { get; }
        public int Quantity { get; }

        public string Status { get; }
        public string Notes { get; }
    }
}
