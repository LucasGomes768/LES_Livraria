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

        public Stock BlockItems(Stock stock, decimal amountBlocked)
        {
            if (stock.StcAvailableAmount < amountBlocked) throw new Exception("A quantidade de itens sendo bloqueados excede a quantidade de itens disponíveis");

            stock.StcAvailableAmount -= amountBlocked;
            stock.StcBlockedAmount += amountBlocked;

            return _stockRepository.Update(stock);
        }
    }
}
