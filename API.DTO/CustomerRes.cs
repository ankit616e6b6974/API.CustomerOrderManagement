namespace API.DTO
{
    public class CustomerRes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? OrderCount { get; set; }
        public decimal? TotalPurchase { get; set; }
        public double? ValueScore { get; set; }
        public List<OrderRes>? OrderRes { get; set; }
    }
}
