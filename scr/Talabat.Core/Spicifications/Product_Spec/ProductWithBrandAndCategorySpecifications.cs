using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Talabat.Core.Entities.Products;

namespace Talabat.Core.Spicifications.Product_Spec
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams) : base(P =>
        (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
        (!specParams.BrandId.HasValue || specParams.BrandId == 0 || P.BrandId == specParams.BrandId.Value) &&
        (!specParams.CategoryId.HasValue || specParams.CategoryId == 0 || P.CategoryId == specParams.CategoryId.Value))
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
            
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;

                    case "priceDesc":
                        AddOrderDesc(P => P.Price);
                        break;

                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
                AddOrderBy(P => P.Name);

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);


        }

        public ProductWithBrandAndCategorySpecifications(int id) : base(p => p.Id == id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);






        }



    }   
}
