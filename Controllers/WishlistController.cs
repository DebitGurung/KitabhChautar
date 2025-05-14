using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using KitabhChauta.Dto;
using Microsoft.AspNetCore.Mvc;
using KitabhChauta.Services;

namespace KitabhChauta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistsController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet("{memberId}")]
        public async Task<ActionResult<WishlistDTO>> GetWishlist(int memberId)
        {
            try
            {
                var wishlist = await _wishlistService.GetWishlistByMemberIdAsync(memberId);
                var wishlistDto = new WishlistDTO
                {
                    WishlistId = wishlist.WishlistId,
                    MemberId = wishlist.MemberId,
                    WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDTO
                    {
                        WishlistItemId = wi.WishlistItemId,
                        BookId = wi.BookId
                    }).ToList()
                };
                return Ok(wishlistDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{memberId}/items")]
        public async Task<ActionResult<WishlistDTO>> AddItemToWishlist(int memberId, [FromBody] WishlistItemDTO wishlistItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _wishlistService.AddItemToWishlistAsync(memberId, wishlistItemDto);
                var wishlist = await _wishlistService.GetWishlistByMemberIdAsync(memberId);
                var wishlistDto = new WishlistDTO
                {
                    WishlistId = wishlist.WishlistId,
                    MemberId = wishlist.MemberId,
                    WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDTO
                    {
                        WishlistItemId = wi.WishlistItemId,
                        BookId = wi.BookId
                    }).ToList()
                };
                return Ok(wishlistDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{memberId}/items/{wishlistItemId}")]
        public async Task<IActionResult> RemoveWishlistItem(int memberId, int wishlistItemId)
        {
            try
            {
                await _wishlistService.RemoveWishlistItemAsync(memberId, wishlistItemId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{memberId}")]
        public async Task<IActionResult> ClearWishlist(int memberId)
        {
            try
            {
                await _wishlistService.ClearWishlistAsync(memberId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}