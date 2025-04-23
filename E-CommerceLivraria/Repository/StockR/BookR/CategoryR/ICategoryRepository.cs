using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR.BookR.CategoryR {
    public interface ICategoryRepository {
        public Category? Get(decimal id);
        public List<Category> GetAll();
    }
}
