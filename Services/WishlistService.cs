using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using KitabhChauta.Dto;
using Microsoft.EntityFrameworkCore;

namespace KitabhChauta.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly KitabhChautariDbContext _context;

        public WishlistService(KitabhChautariDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist> GetWishlistByMemberIdAsync(int memberId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Book)
                .FirstOrDefaultAsync(w => w.MemberId == memberId);

            if (wishlist == null)
            {
                wishlist = await CreateWishlistAsync(memberId);
            }

            return wishlist;
        }

        public async Task<Wishlist> CreateWishlistAsync(int memberId)
        {
            if (!await MemberExistsAsync(memberId))
            {
                throw new ArgumentException("Invalid Member ID");
            }

            var wishlist = new Wishlist
            {
                MemberId = memberId
            };

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return wishlist;
        }

        public async Task AddItemToWishlistAsync(int memberId, WishlistItemDTO wishlistItemDto)
        {
            var wishlist = await GetWishlistByMemberIdAsync(memberId);

            // Check if book exists
            var book = await _context.Books.FindAsync(wishlistItemDto.BookId);
            if (book == null)
            {
                throw new ArgumentException("Invalid Book ID");
            }

            // Check if book is already in wishlist
            if (wishlist.WishlistItems.Any(wi => wi.BookId == wishlistItemDto.BookId))
            {
                return; // Book already in wishlist, no action needed
            }

            var wishlistItem = new WishlistItem
            {
                WishlistId = wishlist.WishlistId,
                BookId = wishlistItemDto.BookId
            };

            wishlist.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveWishlistItemAsync(int memberId, int wishlistItemId)
        {
            var wishlist = await GetWishlistByMemberIdAsync(memberId);
            var wishlistItem = wishlist.WishlistItems.FirstOrDefault(wi => wi.WishlistItemId == wishlistItemId);

            if (wishlistItem == null)
            {
                throw new ArgumentException("Wishlist item not found");
            }

            _context.WishlistItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();
        }

        public async Task ClearWishlistAsync(int memberId)
        {
            var wishlist = await GetWishlistByMemberIdAsync(memberId);
            _context.WishlistItems.RemoveRange(wishlist.WishlistItems);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> MemberExistsAsync(int memberId)
        {
            return await _context.Members.AnyAsync(m => m.MemberId == memberId);
        }
    }
}