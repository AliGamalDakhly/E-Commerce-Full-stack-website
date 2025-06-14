using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Entities;

namespace Bussiness_Logic_Layer.IServices
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll(Expression<Func<Product, bool>>? filter = null, params Expression<Func<Product, object>>[] includes);
        Product AddProduct(Product product);
        Product UpdateProduct(Product product);
        bool DeleteProduct(int? id);
        Product GetProductById(int? id);
    }
}
