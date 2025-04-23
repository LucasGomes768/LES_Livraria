using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.StockR.BookR.CategoryR;

namespace E_CommerceLivraria.Services.StockS.BookS.CategoryS {
    public class CategoryService : ICategoryService {
        private ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        public Category? Get(decimal id) {
            return _categoryRepository.Get(id);
        }

        public List<Category> GetAll() {
            return _categoryRepository.GetAll();
        }
    }
}
