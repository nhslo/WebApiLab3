using WebApiLab3.Models;

namespace WebApiLab3.Services;

public interface IProductService
{
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product Add(Product product);
    bool Delete(int id);
}
