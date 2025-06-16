using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Entities;

namespace Bussiness_Logic_Layer.IServices
{
    public interface IOrderItemService
    {
        IEnumerable<OrderItem> GetAll(Expression<Func<OrderItem, bool>>? filter = null, params Expression<Func<OrderItem, object>>[] includes);
        OrderItem AddOrderItem(OrderItem orderItem);
        OrderItem UpdateOrderItem(OrderItem orderItem);
        bool DeleteOrderItem(int? id);
        OrderItem GetOrderItemById(int? id);
    }
}
