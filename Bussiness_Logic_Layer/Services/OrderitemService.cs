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
    public class OrderItemService: IOrderItemService
    {
        private IGenericRepository<OrderItem, int> _orderItemRepository;

        public OrderItemService(IGenericRepository<OrderItem, int> orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public OrderItem AddOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            orderItem = _orderItemRepository.AddEntity(orderItem);
            return orderItem;
        }

        public bool DeleteOrderItem(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _orderItemRepository.DeleteEntity((int)id);
        }

        public IEnumerable<OrderItem> GetAll(Expression<Func<OrderItem, bool>>? filter = null, params Expression<Func<OrderItem, object>>[] includes)
        {
            return _orderItemRepository.GetAll(filter, includes);
        }

        public OrderItem GetOrderItemById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _orderItemRepository.GetById((int)id);
        }

        public OrderItem UpdateOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            OrderItem? dbOrderItem = _orderItemRepository.GetById(orderItem.Id);
            if (dbOrderItem == null)
                throw new KeyNotFoundException($"OrderItem With ID: {orderItem.Id} Not Found to Be Updated");

            dbOrderItem.Quantity = orderItem.Quantity;

            var updatedOrderItem = _orderItemRepository.UpdateEntity(dbOrderItem);
            return updatedOrderItem;
        }
    }
}
