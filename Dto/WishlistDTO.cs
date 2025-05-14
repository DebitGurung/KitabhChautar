using System.ComponentModel.DataAnnotations;

namespace KitabhChauta.Dto
{
    public class WishlistDTO
    {
        public int WishlistId { get; set; }

        [Required]
        public int MemberId { get; set; }

        public List<WishlistItemDTO> WishlistItems { get; set; } = new List<WishlistItemDTO>();
    }
}