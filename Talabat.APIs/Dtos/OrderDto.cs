using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderDto
    {
        public string? BuyerEmail { get; set; }

        [Required]
        public string BasketId { get; set; } = string.Empty;

        [Required]
        public int DeliveryMethodId { get; set; }

        private AddressDto? _shippingAddress;

        public AddressDto? ShipToAddress { get; set; }

        public AddressDto? ShippingAddress
        {
            get => _shippingAddress ?? ShipToAddress;
            set => _shippingAddress = value;
        }
    }
}
