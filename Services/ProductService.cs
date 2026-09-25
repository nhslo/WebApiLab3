using WebApiLab3.Models;

namespace WebApiLab3.Services;

public class ProductService : IProductService
{
    private static readonly List<Product> _products =
    [
        new Product { Id = 1, Name = "Laptop", Price = 450000 },
        new Product { Id = 2, Name = "Keyboard", Price = 18000 },
        new Product { Id = 3, Name = "Mouse", Price = 9500 }
    ];

    public IEnumerable<Product> GetAll() => _products;

    public Product? GetById(int id) => _products.FirstOrDefault(product => product.Id == id);

    public Product Add(Product product)
    {
        product.Id = _products.Count == 0 ? 1 : _products.Max(item => item.Id) + 1;
        _products.Add(product);
        return product;
    }

    public bool Delete(int id)
    {
        var product = GetById(id);
        if (product is null)
        {
            return false;
        }

        _products.Remove(product);
        return true;
    }
}
