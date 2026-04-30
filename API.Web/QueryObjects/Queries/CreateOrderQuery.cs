using API.Domain.Entities;
using API.DTO;
using API.DTO.Common;
using API.Web.Common.Exceptions;
using API.Web.Infrastructure;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Web.QueryObjects.Queries
{
    public class CreateOrderQuery : IRequest<ApiResponse<OrderRes>>
    {
        public CreateOrderReq OrderRequest { get; }

        public CreateOrderQuery(CreateOrderReq orderRequest)
        {
            OrderRequest = orderRequest;
        }
    };

    public class CreateOrderHandler : IRequestHandler<CreateOrderQuery, ApiResponse<OrderRes>>
    {
        private readonly AppDbContext _db;
        public CreateOrderHandler(AppDbContext db) => _db = db;

        public async Task<ApiResponse<OrderRes>> Handle(CreateOrderQuery queryObject, CancellationToken ct)
        {
            var request = queryObject.OrderRequest;
            CreateOrderValidator val = new CreateOrderValidator();
            ValidationResult? validationResult = val.Validate(request, options =>
            {
                options.IncludeRuleSets("CustomerIdNullOrEmpty");
                options.IncludeRuleSets("ItemsNullOrEmpty");
            });

            if ((validationResult!=null && !validationResult.IsValid) || validationResult == null)
            {
                throw new ValidationException("Validation Error", validationResult != null ? validationResult.Errors : null);
            }

            // Merge duplicate product entries
            var mergedItems = request.Items
                .GroupBy(i => i.ProductId)
                .Select(g => new OrderItemReq() { ProductId = g.Key, Quantity = g.Sum(i => i.Quantity) })
                .ToList();

            // Resolve customer, in real world will get the user id from token
            Customer customer = null;
            if (request.CustomerId.HasValue)
            {
                customer = await _db.Customers.FindAsync([request.CustomerId.Value], ct) ?? throw new NotFoundException(nameof(Customer), request.CustomerId.Value);
            }

            // Only processd one transaction at a time.
            var productIds = mergedItems.Select(i => i.ProductId).OrderBy(id => id).ToList();
            var acquiredLocks = new List<SemaphoreSlim>();

            // If any of the changes fail, then the transaction is rolled back and none of the changes are applied to the database, it ensure atomicity.
            await using var transaction = await _db.Database.BeginTransactionAsync(ct);

            try
            {
                foreach (var id in productIds)
                {
                    var semaphore = InventoryLockProvider.GetLock(id);
                    await semaphore.WaitAsync(ct); // Thread waits here if another user is buying this product
                    acquiredLocks.Add(semaphore);
                }

                // Lock inventory rows by loading with tracking (EF optimistic concurrency)
                var inventories = await _db.Inventories
                    .Where(inv => productIds.Contains(inv.ProductId))
                    .Include(inv => inv.Product)
                    .ToListAsync(ct);

                var products = await _db.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync(ct);

                // Validate all products exist
                foreach (var item in mergedItems)
                {
                    if (!products.Any(p => p.Id == item.ProductId))
                        throw new NotFoundException(nameof(Product), item.ProductId);
                }

                // Check inventory
                foreach (var item in mergedItems)
                {
                    var inv = inventories.FirstOrDefault(i => i.ProductId == item.ProductId);
                    var product = products.First(p => p.Id == item.ProductId);

                    if (inv == null || inv.QuantityAvailable < item.Quantity)
                        throw new BusinessRuleException(string.Format($"Insufficient inventory for '{product.Name}'. Requested: {item.Quantity}.", 
                            product.Name,
                            item.Quantity));
                }

                // Build order
                var orderItems = mergedItems.Select(item =>
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    return new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };
                }).ToList();

                var order = new Order
                {
                    CustomerId = customer.Id,
                    CreatedAtUtc = DateTime.UtcNow,
                    Status = 1,
                    TotalAmount = orderItems.Sum(i => i.Quantity * i.UnitPrice),
                    OrderItems = orderItems
                };

                // Deduct inventory
                foreach (var item in mergedItems)
                {
                    var inv = inventories.First(i => i.ProductId == item.ProductId);
                    inv.QuantityAvailable -= item.Quantity;
                    inv.UpdatedAtUtc = DateTime.UtcNow;
                }

                _db.Orders.Add(order);
                int recordAffected = await _db.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                // Response
                return new ApiResponse<OrderRes>()
                {
                    Data = new OrderRes()
                    {
                        CreatedAtUtc = order.CreatedAtUtc,
                        CustomerId = order.CustomerId,
                        Status = order.Status,
                        TotalAmount = order.TotalAmount
                    }
                };
            }
            catch (Exception)
            {
                throw; // preserves stack trace
            }
            finally
            {
                // ALWAYS release the locks
                foreach (var semaphore in acquiredLocks)
                {
                    semaphore.Release();
                }
            }
        }
    }
}
