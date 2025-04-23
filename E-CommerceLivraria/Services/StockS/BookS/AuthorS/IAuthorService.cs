using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.StockS.BookS.AuthorS {
    public interface IAuthorService {
        public Author? Get(decimal id);
        public List<Author> GetAll();
    }
}
