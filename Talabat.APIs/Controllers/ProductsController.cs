using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Products;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Spicifications.Product_Spec;

namespace Talabat.APIs.Controllers
{
    
    public class ProductsController : BaseApiController
    {
        private readonly IGenaricRepositoriy<Product> _productRepo;
        private readonly IGenaricRepositoriy<ProductBrand> _brandsRepo;
        private readonly IGenaricRepositoriy<ProductCategory> _categoriesRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenaricRepositoriy<Product> ProductRepo,
            IGenaricRepositoriy<ProductBrand> brandsRepo,
            IGenaricRepositoriy<ProductCategory> categoriesRepo,
            IMapper mapper)
        {
            _productRepo = ProductRepo;
            _brandsRepo = brandsRepo;
            _categoriesRepo = categoriesRepo;
            _mapper = mapper;
        }

        [HttpGet]
         public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)
         {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);

            var Products = await _productRepo.GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            var countSpec = new ProductWithFilterationForCountSpecification(specParams);

            var count = await _productRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize, count, data));
         }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id) 
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var product = await _productRepo.GetByIdWithSpecAsync(spec);

            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRepo.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategory()
        {
            var categories = await _categoriesRepo.GetAllAsync();
            return Ok(categories);
        }
    }
}
