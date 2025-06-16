using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Entities;

namespace Bussiness_Logic_Layer.IServices
{
    public interface IShoppingCartService
    {
        IEnumerable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>>? filter = null, params Expression<Func<ShoppingCart, object>>[] includes);
        ShoppingCart AddShoppingCart(ShoppingCart shoppingCart);
        ShoppingCart UpdateShoppingCart(ShoppingCart shoppingCart);
        bool DeleteShoppingCart(int? id);
        ShoppingCart GetShoppingCartById(int? id);
        ShoppingCart GetShoppingCartByUserId(string? id);
    }
}
