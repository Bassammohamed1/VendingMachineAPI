using BusinessLogicLayer.DTOS;
using BusinessLogicLayer.Models;

namespace BusinessLogicLayer.Interfaces
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
