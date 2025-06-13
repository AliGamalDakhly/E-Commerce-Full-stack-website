using System.Linq.Expressions;
using Bussiness_Logic_Layer.IServices;
using Data_Access_Layer.Entities;
using Data_Access_Layer.Repository.IGenericRepository;

namespace Bussiness_Logic_Layer.Services
{
    public class CategoryService : ICategoryService
    {
        private IGenericRepository<Category, int> _categoryRepository;

        public CategoryService(IGenericRepository<Category, int> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Category AddCategory(Category category)
        {
            if(category == null) 
                throw new ArgumentNullException(nameof(category));

            category =  _categoryRepository.AddEntity(category);
            return category;
        }

        public bool DeleteCategory(int? id)
        {
            if(id == null)
                throw new ArgumentNullException(nameof(id));

            return _categoryRepository.DeleteEntity((int)id);
        }

        public IEnumerable<Category> GetAll(Expression<Func<Category, bool>>? filter = null, params Expression<Func<Category, object>>[] includes)
        {
            return _categoryRepository.GetAll(filter, includes);
        }

        public Category GetCategoryById(int? id)
        {
            if(id == null)
                throw new ArgumentNullException(nameof(id));

            return _categoryRepository.GetById((int)id);
        }

        public Category UpdateCategory(Category category)
        {
            if(category == null)
                throw new ArgumentNullException(nameof(category));

            Category? dbCategory = _categoryRepository.GetById(category.Id);
            if (dbCategory == null)
                throw new KeyNotFoundException($"Category With ID: {category.Id} Not Found to Be Updated");
   
            dbCategory.Name = category.Name;
            dbCategory.Description = category.Description;

            var updatedCategory =  _categoryRepository.UpdateEntity(dbCategory);
            return updatedCategory;
        }



    }
}
