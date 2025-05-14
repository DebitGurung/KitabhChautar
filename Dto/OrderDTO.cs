namespace KitabhChauta.Dto
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCanceled { get; set; } // New field
        public DateTime? CancellationDate { get; set; } // New field
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}