using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR.BookR.PublisherR {
    public interface IPublisherRepository {
        public Publisher? Get(decimal id);
        public List<Publisher> GetAll();
    }
}
