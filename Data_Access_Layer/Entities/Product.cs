using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Product Image")]
        public string? Img {  get; set; }
        public decimal Price {  get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;


        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
