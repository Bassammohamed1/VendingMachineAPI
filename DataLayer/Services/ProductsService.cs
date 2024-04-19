using BusinessLogicLayer.DTOS;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataLayer.Services
{
    public class ProductsService : IProductsService
    {
        private readonly AppDbContext _context;
        public ProductsService(AppDbContext context)
        {
            _context = context;
        }
        public List<ProductsDTO> GetAll()
        {
            return _context.Products.Include(x => x.Seller).Select(p => new ProductsDTO
            {
                ProductName = p.ProductName,
                AmountAvailable = p.AmountAvailable,
                Cost = p.Cost,
                Seller = p.Seller.UserName
            }).ToList();
        }
        public Product GetById(int id)
        {
            return _context.Products.Find(id);
        }
        public Product GetByName(string name)
        {
            return _context.Products.Single(x => x.ProductName == name);
        }
        public void Add(ProductDTO data)
        {
            var product = new Product()
            {
                ProductName = data.ProductName,
                AmountAvailable = data.AmountAvailable,
                Cost = data.Cost,
                SellerId = data.SellerId
            };
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void Update(Product data)
        {
            var product = _context.Products.Find(data.ProductId);
            product.ProductName = data.ProductName;
            product.AmountAvailable = data.AmountAvailable;
            product.Cost = data.Cost;
            product.SellerId = data.SellerId;
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var data = _context.Products.Find(id);
            _context.Products.Remove(data);
            _context.SaveChanges();
        }
    }
}
