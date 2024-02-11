using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TheTask.DTOS;
using TheTask.Models;
using TheTask.Services.Interfaces;

namespace TheTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductsService productsService, ILogger<ProductsController> logger)
        {
            this.productsService = productsService;
            _logger = logger;
        }
        [HttpGet("AllProducts")]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Get products endpoint called");
            var data = productsService.GetAll();
            _logger.LogInformation("Products viewed");
            return Ok(data);
        }
        [HttpGet("ProductById/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation("Get product by id endpoint called");
            if (id == 0 || id == null)
            {
                _logger.LogInformation("User entered an invalid id");
                return BadRequest("Invalid id");
            }
            var data = productsService.GetById(id);
            if (data is null)
            {
                _logger.LogInformation("Product is invalid");
                return BadRequest("Product is invalid");
            }
            _logger.LogInformation("Product viewed");
            return Ok(data);
        }
        [HttpGet("ProductByName/{name}")]
        [AllowAnonymous]
        public IActionResult GetByName(string name)
        {
            _logger.LogInformation("Get product by name endpoint called");
            if (name is null)
            {
                _logger.LogInformation("Name is invalid name");
                return BadRequest("Invalid name");
            }
            var data = productsService.GetByName(name);
            if (data is null)
            {
                _logger.LogInformation("Product is invalid");
                return BadRequest("Product is invalid");
            }
            _logger.LogInformation("Product viewed");
            return Ok(data);
        }
        [HttpPost("AddProduct")]
        [Authorize(Roles = "Seller")]
        public IActionResult Add(ProductDTO data)
        {
            _logger.LogInformation("Add product endpoint called");
            if (data is null)
            {
                _logger.LogInformation("Product is invalid");
                return BadRequest("Product is invalid");
            }
            productsService.Add(data);
            _logger.LogInformation("Product has been added");
            return Ok("Product has been added");
        }
        [HttpPut("UpdateProduct/{id}")]
        [Authorize(Roles = "Seller")]
        public IActionResult Update([FromRoute] int id, [FromBody] Product data)
        {
            _logger.LogInformation("Update product endpoint called");
            if (id == null || id == 0)
            {
                _logger.LogInformation("User entered an invalid id");
                return BadRequest("Invalid id");
            }
            var product = productsService.GetById(id);
            if (product is null)
            {
                _logger.LogInformation("Product is invalid");
                return BadRequest("Product is invalid");
            }
            productsService.Update(data);
            _logger.LogInformation("Product has been updated");
            return Ok("Product has been updated");
        }
        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Roles = "Seller")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Delete product endpoint called");
            if (id == null || id == 0)
            {
                _logger.LogInformation("User entered an invalid id");
                return BadRequest("Invalid id");
            }
            var data = productsService.GetById(id);
            if (data is null)
            {
                _logger.LogInformation("Product is invalid");
                return BadRequest("Product is invalid");
            }
            productsService.Delete(id);
            _logger.LogInformation("Product has been deleted");
            return Ok("Product has been deleted");
        }
    }
}
