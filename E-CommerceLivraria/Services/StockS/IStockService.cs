using E_CommerceLivraria.DTO.ChatbotDTO;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.StockS {
    public interface IStockService {
        public Stock? Get(decimal id);
        public Stock? GetByBook(decimal id);
        public List<Stock> GetAll();
        public Stock BlockItems(Stock stock, decimal amountBlocked);
        public List<RelevantBookInfoAI> GetInfoForAI();
    }
}
