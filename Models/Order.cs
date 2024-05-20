using SonseArt.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
namespace SonseArt.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string UserId {  get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public string Status { get; set; }
        public decimal OrderPrice {  get; set; }
        public string Adres {  get; set; }
        public string UserFirstName {  get; set; }
        public string UserLastName { get; set; }
        public string UserPhone {  get; set; }
        public DateTime date {  get; set; }
        public OrderItem? item { get; set; }
        public List<OrderItem>? Items = new List<OrderItem>();

        public decimal Total()
        {
            decimal total = 0;
            foreach (OrderItem item in Items)
            {
                total += item.ProductPrice * item.Quantity;
            }
            return total;
        }
    }
}
