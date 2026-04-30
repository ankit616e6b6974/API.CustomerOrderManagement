using API.Domain.Entities;
using API.DTO;
using API.DTO.Common;
using API.Web.Common.Exceptions;
using API.Web.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Web.QueryObjects.Queries
{
    public class GetCustomerByIdQuery : IRequest<ApiResponse<CustomerRes>>
    {
        public int? CustomerId { get; }

        public GetCustomerByIdQuery(int? custId)
        {
            CustomerId = custId;
        }
    };

    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, ApiResponse<CustomerRes>>
    {
        private readonly AppDbContext _db;
        public GetCustomerByIdHandler(AppDbContext db) => _db = db;

        public async Task<ApiResponse<CustomerRes>> Handle(GetCustomerByIdQuery queryObject, CancellationToken ct)
        {
            int? custId = queryObject.CustomerId;

            if (custId.HasValue)
            {
                var customer = await _db.Customers
                    .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                    .FirstOrDefaultAsync(c => c.Id == custId, ct) ?? throw new NotFoundException(nameof(Customer), custId);

                var (score, count, total) = SharedFunctions.ValueScoreCalculator(customer.Orders);

                var result = new ApiResponse<CustomerRes>()
                {
                    Data = new CustomerRes
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Email = customer.Email,
                        Phone = customer.Phone,
                        OrderCount = count,
                        TotalPurchase = total,
                        ValueScore = Math.Round(score, 4),
                        OrderRes = customer.Orders.Select(i => new OrderRes
                        {
                            Id = i.Id,
                            CreatedAtUtc = i.CreatedAtUtc,
                            Status = i.Status,
                            TotalAmount = i.TotalAmount,
                            Items = i.OrderItems.Select(k => new OrderItemRes
                            {
                                ProductName = k.Product.Name,
                                UnitPrice = k.UnitPrice,
                                Quantity = k.Quantity
                            }).ToList()
                        }).ToList()
                    }
                };

                return result;
            }

            return new ApiResponse<CustomerRes>();
        }
    }
}
