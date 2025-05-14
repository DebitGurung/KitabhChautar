using KitabhChauta.Model;
using KitabhChauta.Dto;

namespace KitabhChauta.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartByMemberIdAsync(int memberId);
        Task<Cart> CreateCartAsync(int memberId);
        Task AddItemToCartAsync(int memberId, CartItemDTO cartItemDto);
        Task UpdateCartItemAsync(int memberId, int cartItemId, int quantity);
        Task RemoveCartItemAsync(int memberId, int cartItemId);
        Task ClearCartAsync(int memberId);
        Task<bool> MemberExistsAsync(int memberId);
    }
}