using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR.BookR.AuthorR {
    public class AuthorRepository : IAuthorRepository {
        private ECommerceDbContext _dbContext;

        public AuthorRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Author? Get(decimal id) {
            return _dbContext.Authors.FirstOrDefault(x => x.BatId == id);
        }

        public List<Author> GetAll() {
            return _dbContext.Authors.ToList();
        }
    }
}
