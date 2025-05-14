using KitabhChauta.Dto;
using KitabhChauta.Model;

namespace KitabhChauta.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int memberId, List<OrderItemDTO> orderItems);
        Task<decimal> CalculateOrderTotalAsync(int memberId, List<OrderItemDTO> orderItems);
        Task<int> GetCompletedOrderCountAsync(int memberId);
        Task CancelOrderAsync(int memberId, int orderId);
        Task<IEnumerable<Order>> GetOrdersByMemberIdAsync(int memberId); // New
        Task<Order?> GetOrderByIdAsync(int memberId, int orderId); // New
    }
}