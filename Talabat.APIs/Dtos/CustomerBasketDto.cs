using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

        public string? ClientSecret { get; set; }

        public string? PaymentIntentId { get; set; }

        public int? DeliveryMethodId { get; set; }

        public decimal? ShippingPrice { get; set; }
    }
}
