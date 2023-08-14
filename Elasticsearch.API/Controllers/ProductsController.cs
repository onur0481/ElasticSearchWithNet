using Elasticsearch.API.DTOs;
using Elasticsearch.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    
    public class ProductsController : BaseController
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductCreateDTO request)
        {
            return CreateActionResult(await _productService.SaveAsync(request));
        }
    }
}
