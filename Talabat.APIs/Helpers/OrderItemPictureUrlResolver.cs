using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderItemPictureUrlResolver(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null 
                ? $"{request.Scheme}://{request.Host}" 
                : _configuration["ApiBaseUrl"]?.TrimEnd('/');

            if (string.IsNullOrEmpty(baseUrl))
                baseUrl = "https://localhost:7099";

            if (string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{baseUrl}/images/products/placeholder.png";

            if (source.Product.PictureUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                source.Product.PictureUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return source.Product.PictureUrl;
            }

            var pictureUrl = source.Product.PictureUrl.TrimStart('/');
            return $"{baseUrl}/{pictureUrl}";
        }
    }
}
