using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.StockR.BookR {
    public class BookRepository : IBookRepository {
        private ECommerceDbContext _dbContext;

        public BookRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Book? Get(decimal id) {
            return _dbContext.Books
                .Include(x => x.BokPrg)
                .Include(x => x.BokBat)
                .Include(x => x.BokPbl)
                .Include(x => x.BcrBcts)
                .FirstOrDefault(x => x.BokId == id);
        }

        public List<Book> GetAll() {
            return _dbContext.Books
                .Include(x => x.BokPrg)
                .Include(x => x.BokBat)
                .Include(x => x.BokPbl)
                .Include(x => x.BcrBcts)
                .ToList();
        }
    }
}
