using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductsCountSpecifications : Specifications<Product>
    {
        public ProductsCountSpecifications(ProductSpecificationsParams specparams)
            : base(
                p => (!specparams.BrandId.HasValue || p.BrandId == specparams.BrandId) &&
                     (!specparams.CategoryId.HasValue || p.CategoryId == specparams.CategoryId) &&
                     (string.IsNullOrEmpty(specparams.Search) || p.Name.ToLower().Contains(specparams.Search))
            )
        {}
    }
}
