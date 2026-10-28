using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Service.Contract;
using System.Security.Claims;
using Address = Talabat.Core.Entities.Order_Aggregate.Address;

namespace Talabat.APIs.Controllers
{
   
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService,
                                IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(System.Security.Claims.ClaimTypes.Email) ?? orderDto.BuyerEmail;
            if (string.IsNullOrEmpty(buyerEmail))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    var tokenCandidate = authHeader.Substring("Bearer ".Length).Trim();
                    if (tokenCandidate.Contains("@")) buyerEmail = tokenCandidate;
                }
            }

            var shippingAddress = orderDto.ShipToAddress ?? orderDto.ShippingAddress;
            var address = _mapper.Map<AddressDto, Address>(shippingAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail ?? "guest@example.com", orderDto.BasketId,
                                                             orderDto.DeliveryMethodId, address);

            if (order is null) return BadRequest(new ApiResponse(400));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUsers([FromQuery] string? email)
        {
            var buyerEmail = User.FindFirstValue(System.Security.Claims.ClaimTypes.Email) ?? email;
            if (string.IsNullOrEmpty(buyerEmail))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    var tokenCandidate = authHeader.Substring("Bearer ".Length).Trim();
                    if (tokenCandidate.Contains("@")) buyerEmail = tokenCandidate;
                }
            }

            if (string.IsNullOrEmpty(buyerEmail))
                return Ok(new List<OrderToReturnDto>());

            var orders = await _orderService.GetOrdersForUsersAsync(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id, [FromQuery] string? email)
        {
            var buyerEmail = User.FindFirstValue(System.Security.Claims.ClaimTypes.Email) ?? email;
            if (string.IsNullOrEmpty(buyerEmail))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    var tokenCandidate = authHeader.Substring("Bearer ".Length).Trim();
                    if (tokenCandidate.Contains("@")) buyerEmail = tokenCandidate;
                }
            }

            var order = await _orderService.GetOrderByIdAsync(id, buyerEmail ?? string.Empty);

            if (order is null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMetods = await _orderService.GetDiliveryMethodsAsync();

            return Ok(deliveryMetods);
        }
    }
}
