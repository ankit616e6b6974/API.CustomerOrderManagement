namespace API.DTO
{
    public class OrderItemRes
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal {  get; set; }
    }
}
