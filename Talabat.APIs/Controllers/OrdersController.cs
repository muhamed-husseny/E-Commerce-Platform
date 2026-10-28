using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Service.Contract;
using Address = Talabat.Core.Entities.Order_Aggregate.Address;

namespace Talabat.APIs.Controllers
{
    [Authorize]
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
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(buyerEmail))
                return Unauthorized(new ApiResponse(401));

            var shippingAddress = orderDto.ShipToAddress ?? orderDto.ShippingAddress;
            var address = _mapper.Map<AddressDto, Address>(shippingAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId,
                                                             orderDto.DeliveryMethodId, address);

            if (order is null) return BadRequest(new ApiResponse(400));

            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUsers()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(buyerEmail))
                return Unauthorized(new ApiResponse(401));

            var orders = await _orderService.GetOrdersForUsersAsync(buyerEmail);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(buyerEmail))
                return Unauthorized(new ApiResponse(401));

            var order = await _orderService.GetOrderByIdAsync(id, buyerEmail);

            if (order is null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [AllowAnonymous]
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDiliveryMethodsAsync();

            return Ok(deliveryMethods);
        }
    }
}
