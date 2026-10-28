using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Products;

namespace Talabat.Repository.Data
{
    public static class StoreDbContextSeed
    {
        public async static Task SeedAsync (StoreDbContext _dbContext)
        {
            if (_dbContext.ProductBrands.Count() == 0)
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/Data Seed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count() > 0)
                {
                    brands = brands.Select(b => new ProductBrand
                    {
                        Name = b.Name
                    }).ToList();

                    foreach (var brand in brands)
                    {
                        _dbContext.ProductBrands.Add(brand);
                    }
                    await _dbContext.SaveChangesAsync();
                } 
            }
            if (_dbContext.ProductCategories.Count() == 0)
            {
                var catedoryData = File.ReadAllText("../Talabat.Repository/Data/Data Seed/Category.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(catedoryData);

                if (categories?.Count() > 0)
                {
                    categories = categories.Select(b => new ProductCategory
                    {
                        Name = b.Name
                    }).ToList();

                    foreach (var category in categories)
                    {
                        _dbContext.ProductCategories.Add(category);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            if (_dbContext.Products.Count() == 0)
            {
                var productsData = File.ReadAllText("../Talabat.Repository/Data/Data Seed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count() > 0)
                {

                    foreach (var product in products)
                    {
                        _dbContext.Set<Product>().Add(product);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            if (_dbContext.DeliveryMethods.Count() == 0)
            {
                var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/Data Seed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods?.Count() > 0)
                {

                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }


        }
    }
}
