using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductPictureUrlResolver(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null 
                ? $"{request.Scheme}://{request.Host}" 
                : _configuration["ApiBaseUrl"]?.TrimEnd('/');

            if (string.IsNullOrEmpty(baseUrl))
                baseUrl = "https://localhost:7099";

            if (string.IsNullOrEmpty(source.PictureUrl))
                return $"{baseUrl}/images/products/placeholder.png";

            if (source.PictureUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                source.PictureUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return source.PictureUrl;
            }

            var pictureUrl = source.PictureUrl.TrimStart('/');
            return $"{baseUrl}/{pictureUrl}";
        }
    }
}
