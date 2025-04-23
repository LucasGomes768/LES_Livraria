using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.StockR.BookR.PublisherR;

namespace E_CommerceLivraria.Services.StockS.BookS.PublisherS {
    public class PublisherService : IPublisherService {
        private IPublisherRepository _publisherRepository;

        public PublisherService(IPublisherRepository publisherRepository) {
            _publisherRepository = publisherRepository;
        }

        public Publisher? Get(decimal id) {
            return _publisherRepository.Get(id);
        }

        public List<Publisher> GetAll() {
            return _publisherRepository.GetAll();
        }
    }
}
