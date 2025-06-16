using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
