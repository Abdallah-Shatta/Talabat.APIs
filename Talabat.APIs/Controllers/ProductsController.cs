using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseAPIController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductCategory> _categoryRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> brandRepo,
            IGenericRepository<ProductCategory> categoryRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _brandRepo = brandRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecificationsParams specparams)
        {
            var includes = new List<Expression<Func<Product, object>>>() { p => p.Brand, p => p.Category };

            Expression<Func<Product, bool>> filter = p => (!specparams.BrandId.HasValue || p.BrandId == specparams.BrandId) &&
                                                          (!specparams.CategoryId.HasValue || p.CategoryId == specparams.CategoryId);

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

            var products = await _productRepo.GetAllWithSpecsAsync(spec);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var includes = new List<Expression<Func<Product, object>>>() { p => p.Brand, p => p.Category };

            Expression<Func<Product, bool>> filter = p => p.Id == id;

            var spec = new Specifications<Product>(filter, includes);

            var product = await _productRepo.GetWithSpecsAsync(spec);

            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            return Ok(await _brandRepo.GetAllAsync());
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            return Ok(await _categoryRepo.GetAllAsync());
        }
    }
}
