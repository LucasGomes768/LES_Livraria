using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.StockR.BookR;

namespace E_CommerceLivraria.Services.StockS.BookS {
    public class BookService : IBookService {
        private IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
        }

        public Book? Get(decimal id) {
            return _bookRepository.Get(id);
        }

        public List<Book> GetAll() {
            return _bookRepository.GetAll();
        }
    }
}
