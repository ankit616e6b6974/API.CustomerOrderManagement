using API.DTO;
using API.DTO.Common;
using API.Web.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Web.QueryObjects.Queries
{
    public class GetAllOrdersQuery : IRequest<ApiResponse<List<OrderRes>>>
    {
        public string? ProductName { get; }
        public GetAllOrdersQuery(string? productName)
        {
            ProductName = productName;
        }
    };

    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, ApiResponse<List<OrderRes>>>
    {
        private readonly AppDbContext _db;
        public GetAllOrdersHandler(AppDbContext db) => _db = db;

        public async Task<ApiResponse<List<OrderRes>>> Handle(GetAllOrdersQuery queryObject, CancellationToken ct)
        {
            var query = _db.Orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.ProductName))
            {
                // Now you can access o.OrderItems because 'query' is still 'Order'
                query = query.Where(o => o.OrderItems.Any(i =>
                    i.Product.Name.Contains(queryObject.ProductName)));
            }

            var orders = await query
                .OrderByDescending(o => o.CreatedAtUtc)
                .Select(o => new OrderRes // Projecting directly to your DTO
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name,
                    CreatedAtUtc = o.CreatedAtUtc,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    Items = o.OrderItems.Select(i => new OrderItemRes
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                    }).ToList()
                }).ToListAsync(ct);

            return new ApiResponse<List<OrderRes>>()
            {
                Data = orders
            };
        }
    }
}
