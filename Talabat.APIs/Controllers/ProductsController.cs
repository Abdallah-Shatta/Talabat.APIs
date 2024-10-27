using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseAPIController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
        {
            var includes = new List<Expression<Func<Product, object>>>() { p => p.Brand, p => p.Category };

            var spec = new Specifications<Product>(includes);

            var products = await _productRepo.GetAllWithSpecsAsync(spec);

            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var includes = new List<Expression<Func<Product, object>>>() { p => p.Brand, p => p.Category };

            Expression<Func<Product, bool>> filter = p => p.Id == id;

            var spec = new Specifications<Product>(filter, includes);

            var product = await _productRepo.GetWithSpecsAsync(spec);

            if (product is null)
                return NotFound();

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }
    }
}
