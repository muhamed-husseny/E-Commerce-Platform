using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto , string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                var baseUrl = _configuration["ApiBaseUrl"]?.TrimEnd('/');
                var pictureUrl = source.PictureUrl.TrimStart('/');
                return $"{baseUrl}/{pictureUrl}";
            }

            return string.Empty;
        }
    }
}
