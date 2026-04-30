using API.DTO.Common;
using API.DTO;
using API.Web.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Web.QueryObjects.Queries
{
    public class GetAllCustomerQuery : IRequest<ApiResponse<List<CustomerRes>>>
    {
    };

    public class GetAllCustomerHandler : IRequestHandler<GetAllCustomerQuery, ApiResponse<List<CustomerRes>>>
    {
        private readonly AppDbContext _db;
        public GetAllCustomerHandler(AppDbContext db) => _db = db;

        public async Task<ApiResponse<List<CustomerRes>>> Handle(GetAllCustomerQuery queryObject, CancellationToken ct)
        {
            var orders = await _db.Customers
            .Select(o => new CustomerRes // Projecting directly to your DTO
            {
                Id = o.Id,
                Name = o.Name,
                Email = o.Email,
                Phone = o.Phone
            })
            .ToListAsync(ct);

            return new ApiResponse<List<CustomerRes>>()
            {
                Data = orders
            };
        }
    }
}
