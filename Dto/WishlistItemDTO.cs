using System.ComponentModel.DataAnnotations;

namespace KitabhChauta.Dto
{
    public class WishlistItemDTO
    {
        public int WishlistItemId { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}