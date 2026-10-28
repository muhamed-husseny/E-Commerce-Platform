using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Products;

namespace Talabat.Core.Spicifications.Product_Spec
{
    public class ProductWithFilterationForCountSpecification : BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams specParams) : base(P =>
        (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search.ToLower())) &&
        (!specParams.BrandId.HasValue || specParams.BrandId == 0 || P.BrandId == specParams.BrandId.Value) &&
        (!specParams.CategoryId.HasValue || specParams.CategoryId == 0 || P.CategoryId == specParams.CategoryId.Value))
        {
            
        }
    }
}
