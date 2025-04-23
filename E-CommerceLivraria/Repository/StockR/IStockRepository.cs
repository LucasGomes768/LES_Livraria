using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.StockR {
    public interface IStockRepository {
        public Stock? Get(decimal id);
        public Stock? GetByBook(decimal id);
        public List<Stock> GetAll();
        public Stock Update(Stock stock);
    }
}
