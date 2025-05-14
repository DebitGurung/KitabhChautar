using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using KitabhChauta.Dto;
using Microsoft.EntityFrameworkCore;

namespace KitabhChauta.Services
{
    public class CartService : ICartService
    {
        private readonly KitabhChautariDbContext _context;

        public CartService(KitabhChautariDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByMemberIdAsync(int memberId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book)
                .FirstOrDefaultAsync(c => c.MemberId == memberId);

            if (cart == null)
            {
                cart = await CreateCartAsync(memberId);
            }

            return cart;
        }

        public async Task<Cart> CreateCartAsync(int memberId)
        {
            if (!await MemberExistsAsync(memberId))
            {
                throw new ArgumentException("Invalid Member ID");
            }

            var cart = new Cart
            {
                MemberId = memberId
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task AddItemToCartAsync(int memberId, CartItemDTO cartItemDto)
        {
            var cart = await GetCartByMemberIdAsync(memberId);

            // Check if book exists
            var book = await _context.Books.FindAsync(cartItemDto.BookId);
            if (book == null)
            {
                throw new ArgumentException("Invalid Book ID");
            }

            // Check if item already exists in cart
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == cartItemDto.BookId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity;
                _context.Entry(existingItem).State = EntityState.Modified;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    BookId = cartItemDto.BookId,
                    Quantity = cartItemDto.Quantity
                };
                cart.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(int memberId, int cartItemId, int quantity)
        {
            var cart = await GetCartByMemberIdAsync(memberId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

            if (cartItem == null)
            {
                throw new ArgumentException("Cart item not found");
            }

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
                _context.Entry(cartItem).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(int memberId, int cartItemId)
        {
            var cart = await GetCartByMemberIdAsync(memberId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

            if (cartItem == null)
            {
                throw new ArgumentException("Cart item not found");
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int memberId)
        {
            var cart = await GetCartByMemberIdAsync(memberId);
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> MemberExistsAsync(int memberId)
        {
            return await _context.Members.AnyAsync(m => m.MemberId == memberId);
        }
    }
}