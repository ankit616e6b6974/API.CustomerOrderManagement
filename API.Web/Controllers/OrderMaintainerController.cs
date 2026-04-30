using API.DTO;
using API.Web.Infrastructure;
using API.Web.QueryObjects.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Web.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderMaintainerController : BaseController
    {
        public OrderMaintainerController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("isApiRunning")]
        public async Task<IActionResult> IsApiRunning()
        {
            return await Task.FromResult<IActionResult>(new JsonResult(true));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string? productName)   //Get all order or search orders
        {
            var result = await CircuitBreaker.Breaker.ExecuteAsync(() => Mediator.Send(new GetAllOrdersQuery(productName)));

            if (result!=null)
            {
                return new JsonResult(result);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderReq request)
        {
            var result = await CircuitBreaker.Breaker.ExecuteAsync(() => Mediator.Send(new CreateOrderQuery(request)));
            if (result != null)
            {
                return new JsonResult(result);
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int id)
        {
            var result = await CircuitBreaker.Breaker.ExecuteAsync(() => Mediator.Send(new DeleteOrderQuery(id)));
            if (result != null)
            {
                return new JsonResult(result);
            }
            return NoContent();
        }


        // Added here because in API part of assigment nothing to give about procut fetch but in Angular its given to diplay products
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await CircuitBreaker.Breaker.ExecuteAsync(() => Mediator.Send(new GetAllProductQuery()));
            if (result != null)
            {
                return new JsonResult(result);
            }
            return NoContent();
        }
    }
}
