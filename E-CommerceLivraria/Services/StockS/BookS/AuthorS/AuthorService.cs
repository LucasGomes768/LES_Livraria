using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.StockR.BookR.AuthorR;

namespace E_CommerceLivraria.Services.StockS.BookS.AuthorS {
    public class AuthorService : IAuthorService {
        private IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository) {
            _authorRepository = authorRepository;
        }

        public Author? Get(decimal id) {
            return _authorRepository.Get(id);
        }

        public List<Author> GetAll() {
            return _authorRepository.GetAll();
        }
    }
}
