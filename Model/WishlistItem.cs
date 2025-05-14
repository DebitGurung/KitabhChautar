using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitabhChauta.Model
{
    public class WishlistItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishlistItemId { get; set; }

        [Required]
        public int WishlistId { get; set; }

        [ForeignKey("WishlistId")]
        public Wishlist Wishlist { get; set; } = null!;

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; } = null!;
    }
}