namespace Api_comerce.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public Accounts User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PaidAt { get; set; }

        public string Status { get; set; }
        public string PaymentMethod { get; set; } // e.g., "CreditCard", "PayPal", etc.
        public string PaymentStatus { get; set; } // e.g., "Pending", "Paid", "Failed"
        public string PaymentReference { get; set; } // Código externo si aplica

        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}
