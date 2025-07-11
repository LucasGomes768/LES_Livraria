﻿using E_CommerceLivraria.DTO.ChatbotDTO;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.StockS {
    public interface IStockService {
        public Stock? Get(decimal id);
        public Stock? GetByBook(decimal id);
        public List<Stock> GetAll();
        public Stock AddToStock(Stock stock, decimal amountAdded, bool addBlocked = false);
        public Stock BlockItems(Stock stock, decimal amountBlocked);
        public Stock RemoveFromBlocked(Stock stock, decimal amountRemoved);
        public List<RelevantBookInfoAI> GetInfoForAI();
    }
}
