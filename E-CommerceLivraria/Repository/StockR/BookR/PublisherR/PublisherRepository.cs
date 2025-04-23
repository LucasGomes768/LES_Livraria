using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR.BookR.PublisherR {
    public class PublisherRepository : IPublisherRepository {
        private ECommerceDbContext _dbContext;

        public PublisherRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Publisher? Get(decimal id) {
            return _dbContext.Publishers.FirstOrDefault(x => x.PblId == id);
        }

        public List<Publisher> GetAll() {
            return _dbContext.Publishers.ToList();
        }
    }
}
