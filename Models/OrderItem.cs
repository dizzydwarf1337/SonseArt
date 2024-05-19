using Microsoft.EntityFrameworkCore;

namespace SonseArt.Models
{
    public class OrderItem
    {
        public string Id { get; set; }
        public int ProductId { get; set; }
        public string OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        [Precision(18, 2)]
        public decimal ProductPrice { get; set; }
    }
}
