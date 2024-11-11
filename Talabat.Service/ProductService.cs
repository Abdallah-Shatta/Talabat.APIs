using System.Linq.Expressions;
using Talabat.Core.Entities;
using Talabat.Core.IServices;
using Talabat.Core.IUnitOfWork;
using Talabat.Core.Specifications;

namespace Talabat.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationsParams specparams)
        {
            var includes = new List<Expression<Func<Product, object>>>() { p => p.Brand, p => p.Category };

            Expression<Func<Product, bool>> filter = p => (!specparams.BrandId.HasValue || p.BrandId == specparams.BrandId) &&
                                                          (!specparams.CategoryId.HasValue || p.CategoryId == specparams.CategoryId) &&
                                                          (string.IsNullOrEmpty(specparams.Search) || p.Name.ToLower().Contains(specparams.Search));

            var spec = new Specifications<Product>(filter, includes);

            if (!string.IsNullOrEmpty(specparams.Sort))
            {
                switch (specparams.Sort)
                {
                    case "priceAsc":
                        spec.OrderBy = p => p.Price;
                        break;
                    case "priceDesc":
                        spec.OrderByDesc = p => p.Price;
                        break;
                    default:
                        spec.OrderBy = p => p.Name;
                        break;
                }
            }
            else
                spec.OrderBy = p => p.Name;

            // total products = 18
            // pageSize = 5
            // pageIndex = 3
            spec.ApplyPagination((specparams.PageIndex - 1) * specparams.PageSize, specparams.PageSize);

            return await _unitOfWork.GetRepository<Product>().GetAllWithSpecsAsync(spec);
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            var includes = new List<Expression<Func<Product, object>>>() { p => p.Brand, p => p.Category };

            Expression<Func<Product, bool>> filter = p => p.Id == id;

            var spec = new Specifications<Product>(filter, includes);

            return await _unitOfWork.GetRepository<Product>().GetWithSpecsAsync(spec);
        }

        public async Task<int> GetCountAsync(ProductSpecificationsParams specparams)
        {
            var countSepc = new ProductsCountSpecifications(specparams);
            return await _unitOfWork.GetRepository<Product>().GetCountAsync(countSepc);
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
            => await _unitOfWork.GetRepository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
            => await _unitOfWork.GetRepository<ProductCategory>().GetAllAsync();   
    }
}
