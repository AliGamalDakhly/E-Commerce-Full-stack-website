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
    public class ShoppingCartService: IShoppingCartService
    {
        private IGenericRepository<ShoppingCart, int> _shoppingCartRepository;

        public ShoppingCartService(IGenericRepository<ShoppingCart, int> shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public ShoppingCart AddShoppingCart(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                throw new ArgumentNullException(nameof(shoppingCart));

            shoppingCart = _shoppingCartRepository.AddEntity(shoppingCart);
            return shoppingCart;
        }

        public bool DeleteShoppingCart(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _shoppingCartRepository.DeleteEntity((int)id);
        }

        public IEnumerable<ShoppingCart> GetAll(Expression<Func<ShoppingCart, bool>>? filter = null, params Expression<Func<ShoppingCart, object>>[] includes)
        {
            return _shoppingCartRepository.GetAll(filter, includes);
        }

        public ShoppingCart GetShoppingCartById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _shoppingCartRepository.GetById((int)id);
        }

        public ShoppingCart GetShoppingCartByUserId(string? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _shoppingCartRepository.GetAll(sc => sc.AppUserId == id).FirstOrDefault();
        }

        #region Update Shopping Cart
        public ShoppingCart UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                throw new ArgumentNullException(nameof(shoppingCart));

            ShoppingCart? dbShoppingCart = _shoppingCartRepository.GetById(shoppingCart.Id);
            if (dbShoppingCart == null)
                throw new KeyNotFoundException($"ShoppingCart With ID: {shoppingCart.Id} Not Found to Be Updated");

            dbShoppingCart.OrderItems = shoppingCart.OrderItems;

            var updatedShoppingCart = _shoppingCartRepository.UpdateEntity(dbShoppingCart);
            return updatedShoppingCart;
        }
        #endregion


    }
}
