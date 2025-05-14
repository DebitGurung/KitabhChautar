using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using KitabhChauta.Dto;
using Microsoft.AspNetCore.Mvc;

namespace KitabhChauta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{memberId}")]
        public async Task<ActionResult<CartDTO>> GetCart(int memberId)
        {
            try
            {
                var cart = await _cartService.GetCartByMemberIdAsync(memberId);
                var cartDto = new CartDTO
                {
                    CartId = cart.CartId,
                    MemberId = cart.MemberId,
                    CartItems = cart.CartItems.Select(ci => new CartItemDTO
                    {
                        CartItemId = ci.CartItemId,
                        BookId = ci.BookId,
                        Quantity = ci.Quantity
                    }).ToList()
                };
                return Ok(cartDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{memberId}/items")]
        public async Task<ActionResult<CartDTO>> AddItemToCart(int memberId, [FromBody] CartItemDTO cartItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _cartService.AddItemToCartAsync(memberId, cartItemDto);
                var cart = await _cartService.GetCartByMemberIdAsync(memberId);
                var cartDto = new CartDTO
                {
                    CartId = cart.CartId,
                    MemberId = cart.MemberId,
                    CartItems = cart.CartItems.Select(ci => new CartItemDTO
                    {
                        CartItemId = ci.CartItemId,
                        BookId = ci.BookId,
                        Quantity = ci.Quantity
                    }).ToList()
                };
                return Ok(cartDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{memberId}/items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int memberId, int cartItemId, [FromBody] int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be positive");
            }

            try
            {
                await _cartService.UpdateCartItemAsync(memberId, cartItemId, quantity);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{memberId}/items/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int memberId, int cartItemId)
        {
            try
            {
                await _cartService.RemoveCartItemAsync(memberId, cartItemId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{memberId}")]
        public async Task<IActionResult> ClearCart(int memberId)
        {
            try
            {
                await _cartService.ClearCartAsync(memberId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}