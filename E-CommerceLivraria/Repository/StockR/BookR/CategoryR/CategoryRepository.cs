using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR.BookR.CategoryR {
    public class CategoryRepository : ICategoryRepository {
        private readonly ECommerceDbContext _dbContext;

        public CategoryRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Category? Get(decimal id) {
            return _dbContext.Categories.FirstOrDefault(x => x.BctId == id);
        }

        public List<Category> GetAll() {
            return _dbContext.Categories.ToList();
        }
    }
}
