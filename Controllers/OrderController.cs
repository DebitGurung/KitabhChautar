using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using KitabhChauta.Dto;
using Microsoft.AspNetCore.Mvc;

namespace KitabhChauta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public OrdersController(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        [HttpPost("{memberId}")]
        public async Task<ActionResult<OrderDTO>> CreateOrder(int memberId)
        {
            var cart = await _cartService.GetCartByMemberIdAsync(memberId);
            if (!cart.CartItems.Any())
            {
                return BadRequest("Cart is empty");
            }

            var orderItems = cart.CartItems.Select(ci => new OrderItemDTO
            {
                BookId = ci.BookId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Book.Price
            }).ToList();

            var order = await _orderService.CreateOrderAsync(memberId, orderItems);

            // Clear the cart after order creation
            await _cartService.ClearCartAsync(memberId);

            var orderDto = new OrderDTO
            {
                OrderId = order.OrderId,
                MemberId = order.MemberId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                IsCompleted = order.IsCompleted,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    OrderItemId = oi.OrderItemId,
                    BookId = oi.BookId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            return CreatedAtAction(nameof(CreateOrder), new { memberId }, orderDto);
        }
    }
}