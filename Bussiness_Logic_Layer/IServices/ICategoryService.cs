using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Entities;

namespace Bussiness_Logic_Layer.IServices
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll(Expression<Func<Category, bool>>? filter = null, params Expression<Func<Category, object>>[] includes);
        Category AddCategory(Category category);
        Category UpdateCategory(Category category);
        bool DeleteCategory(int? id);
        Category GetCategoryById(int? id);
    }
}
