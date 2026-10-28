namespace Talabat.Core.Entities.Products
{
    public class BasketItem
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string PictureUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}