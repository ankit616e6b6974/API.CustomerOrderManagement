namespace API.DTO
{
    public class OrderRes
    {
        public int Id {  get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedAtUtc {  get; set; }
        public int Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemRes>? Items { get; set; }
    }
}
