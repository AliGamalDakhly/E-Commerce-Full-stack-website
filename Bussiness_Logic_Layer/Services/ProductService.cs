using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bussiness_Logic_Layer.IServices;
using Data_Access_Layer.Entities;
using Data_Access_Layer.Repository.IGenericRepository;

namespace Bussiness_Logic_Layer.Services
{
    public class ProductService: IProductService
    {
        private IGenericRepository<Product, int> _productRepository;

        public ProductService(IGenericRepository<Product, int> productRepository)
        {
            _productRepository = productRepository;
        }

        public Product AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            product = _productRepository.AddEntity(product);
            return product;
        }

        public bool DeleteProduct(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _productRepository.DeleteEntity((int)id);
        }

        public IEnumerable<Product> GetAll(Expression<Func<Product, bool>>? filter = null, params Expression<Func<Product, object>>[] includes)
        {
            return _productRepository.GetAll(filter, includes);
        }

        public Product GetProductById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _productRepository.GetById((int)id);
        }

        public Product UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            Product? dbProduct = _productRepository.GetById(product.Id);
            if (dbProduct == null)
                throw new KeyNotFoundException($"Product With ID: {product.Id} Not Found to Be Updated");

            dbProduct.Name = product.Name;
            dbProduct.Description = product.Description;
            dbProduct.Price = product.Price;
            dbProduct.Img = product.Img;
            dbProduct.CategoryId = product.CategoryId;

            var updatedProduct = _productRepository.UpdateEntity(dbProduct);
            return updatedProduct;
        }
    }
}
