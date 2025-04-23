using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.StockS.BookS {
    public interface IBookService {
        public Book? Get(decimal id);
        public List<Book> GetAll();
    }
}
