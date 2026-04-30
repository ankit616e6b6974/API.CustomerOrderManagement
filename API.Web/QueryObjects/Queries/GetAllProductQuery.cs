using API.DTO;
using API.DTO.Common;
using API.Web.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Web.QueryObjects.Queries
{
    public class GetAllProductQuery : IRequest<ApiResponse<List<ProductRes>>>
    {
    };

    public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, ApiResponse<List<ProductRes>>>
    {
        private readonly AppDbContext _db;
        public GetAllProductHandler(AppDbContext db) => _db = db;

        public async Task<ApiResponse<List<ProductRes>>> Handle(GetAllProductQuery queryObject, CancellationToken ct)
        {
            var orders = await _db.Products
            .Include(p => p.Inventory)
            .Where(p => p.Inventory != null && p.Inventory.QuantityAvailable > 0)
            .Select(o => new ProductRes // Projecting directly to your DTO
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                Price = o.Price
            })
            .ToListAsync(ct);

            return new ApiResponse<List<ProductRes>>()
            {
                Data = orders
            };
        }
    }
}
