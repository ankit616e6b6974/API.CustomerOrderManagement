using API.Web.Infrastructure;
using API.Web.QueryObjects.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Web.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerMaintainerController : BaseController
    {
        public CustomerMaintainerController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("isApiRunning")]
        public async Task<IActionResult> IsApiRunning()
        {
            return await Task.FromResult<IActionResult>(new JsonResult(true));
        }

        [HttpGet("top")]
        public async Task<IActionResult> GetTopCustomers([FromQuery] int? count) 
        {
            var result = await CircuitBreaker.Breaker.ExecuteAsync(() => Mediator.Send(new GetTopCustomersQuery(count)));

            if (result!=null)
            {
                return new JsonResult(result);
            }
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int custId)
        {
            var result = await CircuitBreaker.Breaker.ExecuteAsync(() => Mediator.Send(new GetCustomerByIdQuery(custId)));
            if (result != null)
            {
                return new JsonResult(result);
            }
            return NoContent();
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await CircuitBreaker.Breaker.ExecuteAsync(() => Mediator.Send(new GetAllCustomerQuery()));
            if (result != null)
            {
                return new JsonResult(result);
            }
            return NoContent();
        }
    }
}
