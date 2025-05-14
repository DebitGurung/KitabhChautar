using KitabhChauta.Interfaces;
using KitabhChauta.Model;
using KitabhChauta.Dto;
using Microsoft.EntityFrameworkCore;

namespace KitabhChauta.Services
{
    public class OrderService : IOrderService
    {
        private readonly KitabhChautariDbContext _context;
        private readonly IBookService _bookService;

        public OrderService(KitabhChautariDbContext context, IBookService bookService)
        {
            _context = context;
            _bookService = bookService;
        }

        public async Task<Order> CreateOrderAsync(int memberId, List<OrderItemDTO> orderItems)
        {
            if (!await _context.Members.AnyAsync(m => m.MemberId == memberId))
            {
                throw new ArgumentException("Invalid Member ID");
            }

            var order = new Order
            {
                MemberId = memberId,
                OrderDate = DateTime.UtcNow,
                IsCompleted = true // Assume order is completed upon creation
            };

            foreach (var item in orderItems)
            {
                var book = await _context.Books.FindAsync(item.BookId);
                if (book == null)
                {
                    throw new ArgumentException($"Invalid Book ID: {item.BookId}");
                }
                if (book.StockCount < item.Quantity)
                {
                    throw new ArgumentException($"Insufficient stock for book: {book.Title}");
                }

                order.OrderItems.Add(new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    UnitPrice = book.Price
                });

                book.StockCount -= item.Quantity;
                _context.Entry(book).State = EntityState.Modified;
            }

            order.TotalPrice = await CalculateOrderTotalAsync(memberId, orderItems);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<decimal> CalculateOrderTotalAsync(int memberId, List<OrderItemDTO> orderItems)
        {
            decimal total = 0;
            int totalBooks = 0;

            foreach (var item in orderItems)
            {
                var book = await _context.Books.FindAsync(item.BookId);
                if (book == null)
                {
                    throw new ArgumentException($"Invalid Book ID: {item.BookId}");
                }

                var price = book.Price;
                if (book.IsOnSale && book.DiscountPercentage.HasValue)
                {
                    price *= (1 - book.DiscountPercentage.Value);
                }

                total += price * item.Quantity;
                totalBooks += item.Quantity;
            }

            if (totalBooks >= 5)
            {
                total *= 0.95m; // 5% discount
            }

            var completedOrders = await GetCompletedOrderCountAsync(memberId);
            if (completedOrders >= 10)
            {
                total *= 0.90m; // 10% additional discount
            }

            return total;
        }

        public async Task<int> GetCompletedOrderCountAsync(int memberId)
        {
            return await _context.Orders
                .CountAsync(o => o.MemberId == memberId && o.IsCompleted && !o.IsCanceled);
        }

        public async Task CancelOrderAsync(int memberId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.MemberId == memberId);

            if (order == null)
            {
                throw new ArgumentException("Order not found or does not belong to the member");
            }

            if (order.IsCompleted)
            {
                throw new ArgumentException("Cannot cancel a completed order");
            }

            if (order.IsCanceled)
            {
                throw new ArgumentException("Order is already canceled");
            }

            var timeSinceOrder = DateTime.UtcNow - order.OrderDate;
            if (timeSinceOrder.TotalHours > 24)
            {
                throw new ArgumentException("Order can only be canceled within 24 hours of placement");
            }

            order.IsCanceled = true;
            order.CancellationDate = DateTime.UtcNow;

            foreach (var item in order.OrderItems)
            {
                var book = item.Book;
                book.StockCount += item.Quantity;
                _context.Entry(book).State = EntityState.Modified;
            }

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByMemberIdAsync(int memberId)
        {
            if (!await _context.Members.AnyAsync(m => m.MemberId == memberId))
            {
                throw new ArgumentException("Invalid Member ID");
            }

            return await _context.Orders
                .Where(o => o.MemberId == memberId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int memberId, int orderId)
        {
            if (!await _context.Members.AnyAsync(m => m.MemberId == memberId))
            {
                throw new ArgumentException("Invalid Member ID");
            }

            return await _context.Orders
                .Where(o => o.MemberId == memberId && o.OrderId == orderId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync();
        }
    }
}