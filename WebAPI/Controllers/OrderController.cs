using Application.Features.Commands.Brands.CreateBrand;
using Application.Features.Commands.Orders.CreateOrder;
using Application.Features.Queries.Genders.GetAllGender;
using Application.Features.Queries.Orders.GetAllOrder;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrderQueryRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommandRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
