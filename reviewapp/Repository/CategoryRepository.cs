using Microsoft.EntityFrameworkCore.Diagnostics;
using reviewapp.Data;
using reviewapp.Interfaces;
using reviewapp.Model;

namespace reviewapp.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly DataContext _categoryRepository;
        public CategoryRepository(DataContext dataContext)
        {
            _categoryRepository = dataContext;  
        }
        public bool CategoryExists(int id)
        {
           var category =  _categoryRepository.Categories.Any(c => c.Id == id);
            return category;
        }
        public bool CreateCategory(Category category)
        {
            _categoryRepository.Categories.Add(category);
            return Save();
        }
        public bool DeleteCategory(Category category)
        {
            _categoryRepository.Categories.Remove(category);
            return Save();
        }
        public ICollection<Category> GetCategories()
        {
            var categories = _categoryRepository.Categories.ToList();
            return categories;  
         }
        public Category GetCategory(int id)
        {
            return _categoryRepository.Categories.Where(c => c.Id == id).FirstOrDefault();
        }
        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
           return  _categoryRepository.PokemonCategories.Where(e=> e.CategoryId == categoryId).Select(c=> c.Pokemon).ToList() ;

        }
        public bool UpdateCategory(Category category)
        {
            _categoryRepository.Categories.Update(category);
            return Save();
        }
        public bool Save()
        {
            var saved = _categoryRepository.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
