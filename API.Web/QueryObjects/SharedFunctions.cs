using API.Domain.Entities;

namespace API.Web.QueryObjects
{
    public class SharedFunctions
    {
        public const int MaxFrequency = 5;
        public const decimal MaxMonetary = 6000m;


        public static (double ValueScore, int OrderCount, decimal TotalPurchase) ValueScoreCalculator(
            IEnumerable<Order> orders)
        {
            var confirmedOrders = orders
                .Where(o => o.Status == 1)
                .ToList();

            var orderCount = confirmedOrders.Count;
            var totalPurchase = confirmedOrders.Sum(o => o.TotalAmount);

            var frequencyRatio = Math.Min((double)orderCount / MaxFrequency, 1.0);
            var monetaryRatio = Math.Min((double)(totalPurchase / MaxMonetary), 1.0);

            var valueScore = frequencyRatio * 0.4 + monetaryRatio * 0.6;

            return (Math.Round(valueScore, 4), orderCount, totalPurchase);
        }
    }
}
