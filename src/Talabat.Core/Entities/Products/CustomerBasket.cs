using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Products
{
    public class CustomerBasket
    {
        public string Id { get; set; } = string.Empty;
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }

        public CustomerBasket()
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;
            Items = new List<BasketItem>();
        }
    }

}
