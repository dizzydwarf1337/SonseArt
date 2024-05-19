using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace SonseArt.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        [ForeignKey(nameof(Cart))]
        public string CartId { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId {  get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        [Precision(18, 2)]
        public decimal ProductPrice { get; set; }
        public string? ImgSrc {  get; set; }
    }
}
