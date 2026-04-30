using API.Domain.Entities;
using API.DTO.Common;
using API.Web.Common.Exceptions;
using API.Web.Infrastructure;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Web.QueryObjects.Queries
{
    public class DeleteOrderQuery : IRequest<ApiResponse<bool>>
    {
        public int? OrderId { get; }

        public DeleteOrderQuery(int? orderid)
        {
            OrderId = orderid;
        }
    };

    public class DeleteOrderHandler : IRequestHandler<DeleteOrderQuery, ApiResponse<bool>>
    {
        private readonly AppDbContext _db;
        public DeleteOrderHandler(AppDbContext db) => _db = db;

        public async Task<ApiResponse<bool>> Handle(DeleteOrderQuery queryObject, CancellationToken ct)
        {
            int? ordId = queryObject.OrderId;

            if (!ordId.HasValue)
            {
                var failure = new ValidationFailure("Input", "Input can not be null.");

                throw new ValidationException(new[] { failure });
            }

            var order = await _db.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == ordId, ct) ?? throw new NotFoundException(nameof(Order), ordId);

            if (order.Status == 2)
                throw new BusinessRuleException("Order is already cancelled and cannot be cancelled again.");

            // If any of the changes fail, then the transaction is rolled back and none of the changes are applied to the database, it ensure atomicity.
            await using var transaction = await _db.Database.BeginTransactionAsync(ct);

            try
            {
                //Restock inventory
                var productIds = order.OrderItems.Select(i => i.ProductId).ToList();
                var inventories = await _db.Inventories
                    .Where(inv => productIds.Contains(inv.ProductId))
                    .ToListAsync(ct);

                foreach (var item in order.OrderItems)
                {
                    var inv = inventories.FirstOrDefault(i => i.ProductId == item.ProductId);
                    if (inv != null)
                    {
                        inv.QuantityAvailable += item.Quantity;
                        inv.UpdatedAtUtc = DateTime.UtcNow;
                    }
                }

                order.Status = 2;
                int recordAffected = await _db.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return new ApiResponse<bool>()
                {
                    Data = (recordAffected > 0)
                };
            }
            catch (Exception)
            {
                throw; // preserves stack trace
            }
        }
    }
}
