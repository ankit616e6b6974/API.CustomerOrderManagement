using API.DTO;
using API.DTO.Common;
using API.Web.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Web.QueryObjects.Queries
{
    public class GetTopCustomersQuery : IRequest<ApiResponse<List<CustomerRes>>>
    {
        public int? Count = null;

        public GetTopCustomersQuery(int? count)
        {
            Count = count;
        }
    };

    public class GetTopCustomersHandler : IRequestHandler<GetTopCustomersQuery, ApiResponse<List<CustomerRes>>>
    {
        private readonly AppDbContext _db;
        public GetTopCustomersHandler(AppDbContext db) => _db = db;

        public async Task<ApiResponse<List<CustomerRes>>> Handle(GetTopCustomersQuery queryObject, CancellationToken ct)
        {
            const int maxFrequency = SharedFunctions.MaxFrequency;
            const decimal maxMonetary = SharedFunctions.MaxMonetary;
            const int count = 3;

            // Use FromSqlInterpolated for safe, readable parameterization
            var top3 = await _db.Database
                .SqlQuery<CustomerScore>($@"
                    WITH ScoredCustomers AS (
                        SELECT 
                            c.Id, c.Name, c.Email, c.Phone,
                            COUNT(CASE WHEN o.Status = 1 THEN 1 END) as OrderCount,
                            COALESCE(SUM(CASE WHEN o.Status = 1 THEN o.TotalAmount END), 0) as TotalPurchase
                        FROM Customers c
                        INNER JOIN Orders o ON c.Id = o.CustomerId
                        GROUP BY c.Id, c.Name, c.Email, c.Phone
                    ),
                    RankedCustomers AS (
                        SELECT *,
                            ((CASE WHEN OrderCount >= {maxFrequency} THEN 1.0 ELSE CAST(OrderCount AS int) / {maxFrequency} END) * 0.4) +
                            ((CASE WHEN TotalPurchase >= {maxMonetary} THEN 1.0 ELSE CAST(TotalPurchase AS float) / {maxMonetary} END) * 0.6) 
                            as ValueScore
                        FROM ScoredCustomers
                    ),
                    FinalRanking AS (
                        SELECT *,
                            DENSE_RANK() OVER (ORDER BY ValueScore DESC) as ScoreRank
                        FROM RankedCustomers
                    )
                    SELECT Id, Name, Email, Phone, OrderCount, TotalPurchase, ROUND(ValueScore, 4) as ValueScore
                    FROM FinalRanking
                    WHERE ScoreRank <= {count}
                    ORDER BY ValueScore DESC"
                ).ToListAsync(ct);


            // Another approach
            // Add a 2 new column in customer table, TotalSpend and OrderFrequencyCounter
            // Once we have this 2 field we can directly do this 

            //var topGroups = await _db.Customers
            //.Select(c => new
            //{
            //    Customer = c,
            //    // Calculate score once per customer
            //    ValueScore = (c.TotalOrderCount >= maxFrequency ? 1.0 : (double)c.TotalOrderCount / maxFrequency) * 0.4
            //               + (c.TotalSpend >= maxMonetary ? 1.0 : (double)c.TotalSpend / maxMonetary) * 0.6
            //})
            //// Group by the score to identify "Ties"
            //.GroupBy(x => x.ValueScore)
            //// Order groups by the score descending
            //.OrderByDescending(g => g.Key)
            //// Take the top 3 score groups
            //.Take(3)
            //// Flatten the groups back into a list of customers
            //.SelectMany(g => g.Select(x => new CustomerRes
            //{
            //    Id = x.Customer.Id,
            //    Name = x.Customer.Name,
            //    Email = x.Customer.Email,
            //    Phone = x.Customer.Phone,
            //    OrderCount = x.Customer.TotalOrderCount,
            //    TotalPurchase = x.Customer.TotalSpend,
            //    ValueScore = Math.Round(x.ValueScore, 4)
            //}))
            //.ToListAsync(ct);

            var result = top3.Select(c => new CustomerRes
            {
                Id = c.Id, 
                Name = c.Name, 
                Email = c.Email, 
                Phone = c.Phone,
                OrderCount = c.OrderCount, 
                TotalPurchase = c.TotalPurchase,
                ValueScore = c.ValueScore
            }).ToList();

            return new ApiResponse<List<CustomerRes>>
            {
                Data = result
            };
        }
    }
}
