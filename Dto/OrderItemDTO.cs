namespace KitabhChauta.Dto
{
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
