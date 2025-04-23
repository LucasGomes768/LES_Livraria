using System.Runtime.CompilerServices;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR.BookR {
    public interface IBookRepository {
        public Book? Get(decimal id);
        public List<Book> GetAll();
    }
}
