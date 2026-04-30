using API.Domain.Entities;
using API.DTO;
using API.Web.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Web.CommandQueryObjects.Queries
{
    public class GetAllOrdersQuery() : IRequest<List<OrderRes>>;

    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<OrderRes>>
    {
        private readonly AppDbContext _db;
        public GetAllOrdersHandler(AppDbContext db) => _db = db;

        public async Task<List<OrderRes>> Handle(GetAllOrdersQuery request, CancellationToken ct)
        {
            IQueryable<OrderRes> query = _db.Orders
                .OrderByDescending(o => o.CreatedAtUtc)
                .Select(o => new OrderRes // Projecting directly to your DTO
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name, // EF joins Customers and only selects 'Name'
                    CreatedAtUtc = o.CreatedAtUtc,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    Items = o.OrderItems.Select(i => new OrderItemRes
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name, // EF joins Products and only selects 'Name'
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                    }).ToList()
                });

            var orders = await query.ToListAsync(ct);

            return orders;
        }
    }
}
