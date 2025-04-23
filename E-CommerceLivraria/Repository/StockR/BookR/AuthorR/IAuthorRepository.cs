using System.Runtime.CompilerServices;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR.BookR.AuthorR {
    public interface IAuthorRepository {
        public Author? Get(decimal id);
        public List<Author> GetAll();
    }
}
