using SonseArt.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace SonseArt.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ShoppingCart")]
        public User User { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal GetCost()
        {
            decimal cost = 0;
            foreach(var item in Items)
            {
                cost += item.ProductPrice*item.Quantity;
            }
            return cost;
        }
    }
}
