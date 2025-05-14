using System.ComponentModel.DataAnnotations;

namespace KitabhChauta.Dto
{
    public class CartDTO
    {
        public int CartId { get; set; }

        [Required]
        public int MemberId { get; set; }

        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }
}