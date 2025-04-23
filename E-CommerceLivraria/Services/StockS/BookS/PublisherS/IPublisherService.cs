using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.StockS.BookS.PublisherS {
    public interface IPublisherService {
        public Publisher? Get(decimal id);
        public List<Publisher> GetAll();
    }
}
