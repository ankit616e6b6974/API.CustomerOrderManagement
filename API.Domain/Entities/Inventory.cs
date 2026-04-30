namespace API.Domain.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int QuantityAvailable { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public Product Product { get; set; }
    }
}
