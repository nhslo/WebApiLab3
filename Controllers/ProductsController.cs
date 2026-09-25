using Microsoft.AspNetCore.Mvc;
using WebApiLab3.Models;
using WebApiLab3.Services;

namespace WebApiLab3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        _logger.LogInformation("Getting all products");
        return Ok(_productService.GetAll());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Product> GetById(int id)
    {
        try
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid product ID {ProductId}", id);
                return BadRequest("Product ID must be positive.");
            }

            var product = _productService.GetById(id);
            if (product is null)
            {
                _logger.LogWarning("Product with ID {ProductId} was not found", id);
                return NotFound();
            }

            _logger.LogInformation("Product with ID {ProductId} was found", id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing product with ID {ProductId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
        }
    }

    [HttpPost]
    public ActionResult<Product> Create(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0)
        {
            _logger.LogWarning("Invalid product data received");
            return BadRequest("Name is required and Price must be greater than zero.");
        }

        var createdProduct = _productService.Add(product);
        _logger.LogInformation("Product {ProductName} was created", createdProduct.Name);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!_productService.Delete(id))
        {
            _logger.LogWarning("Product with ID {ProductId} was not found", id);
            return NotFound();
        }

        _logger.LogInformation("Product with ID {ProductId} was deleted", id);
        return NoContent();
    }
}
