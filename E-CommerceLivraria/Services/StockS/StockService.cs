using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.StockR;

namespace E_CommerceLivraria.Services.StockS {
    public class StockService : IStockService {
        private IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository) {
            _stockRepository = stockRepository;
        }

        public Stock? Get(decimal id) {
            return _stockRepository.Get(id);
        }

        public List<Stock> GetAll() {
            return _stockRepository.GetAll();
        }

        public Stock? GetByBook(decimal id) {
            return _stockRepository.GetByBook(id);
        }

        public Stock Update(Stock stock) {
            return _stockRepository.Update(stock);
        }
    }
}
