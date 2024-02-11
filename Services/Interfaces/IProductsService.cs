using System.Linq.Expressions;
using TheTask.DTOS;
using TheTask.Models;

namespace TheTask.Services.Interfaces
{
    public interface IProductsService
    {
        List<ProductsDTO> GetAll();
        Product GetById(int id);
        Product GetByName(string name);
        void Add(ProductDTO data);
        void Update(Product data);
        void Delete(int id);
    }
}
