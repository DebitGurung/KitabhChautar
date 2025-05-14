using KitabhChauta.Model;
using KitabhChauta.Dto;

namespace KitabhChauta.Interfaces
{
    public interface IWishlistService
    {
        Task<Wishlist> GetWishlistByMemberIdAsync(int memberId);
        Task<Wishlist> CreateWishlistAsync(int memberId);
        Task AddItemToWishlistAsync(int memberId, WishlistItemDTO wishlistItemDto);
        Task RemoveWishlistItemAsync(int memberId, int wishlistItemId);
        Task ClearWishlistAsync(int memberId);
        Task<bool> MemberExistsAsync(int memberId);
    }
}