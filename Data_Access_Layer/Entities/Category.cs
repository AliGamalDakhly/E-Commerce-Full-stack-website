using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;



        public virtual List<Product> Products { get; set; }

    }
}
