using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.StockS.BookS.CategoryS {
    public interface ICategoryService {
        public Category? Get(decimal id);
        public List<Category> GetAll();
    }
}
