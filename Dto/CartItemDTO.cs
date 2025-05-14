using System.ComponentModel.DataAnnotations;

namespace KitabhChauta.Dto
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}