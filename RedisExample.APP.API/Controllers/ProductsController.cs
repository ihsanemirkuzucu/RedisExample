using Microsoft.AspNetCore.Mvc;
using RedisExample.APP.API.Model;
using RedisExample.APP.API.Repository;
using RedisExample.APP.API.Services;

namespace RedisExample.APP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await productService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await productService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            return Created(string.Empty, await productService.CreateAsync(product));
        }

    }
}
