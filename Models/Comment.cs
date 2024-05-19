using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SonseArt.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Text {  get; set; }
        [Required]
        [ForeignKey("AuthorId")]
        public string AuthorId {  get; set; }
        public string Author {  get; set; }
        public DateTime Created { get; set; }

        [ForeignKey(nameof(ProductId))]
        public int ProductId {  get; set; }

    }
}
