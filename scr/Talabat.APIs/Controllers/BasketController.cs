using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Products;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(
            IBasketRepository basketRepository,
            IMapper mapper
            )
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasket([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
                return Ok(new CustomerBasket());

            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (createdOrUpdatedBasket is null) 
                return BadRequest(new ApiResponse(400));

            return Ok(createdOrUpdatedBasket);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteBasket([FromQuery] string id)
        {
            return Ok(await _basketRepository.DeleteBasketAsync(id));
        }

    }
}
