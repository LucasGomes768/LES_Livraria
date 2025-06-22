using E_CommerceLivraria.DTO.ChatbotDTO;
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

        public List<RelevantBookInfoAI> GetInfoForAI()
        {
            var allBooks = _stockRepository.GetAll();

            List<RelevantBookInfoAI> infos = new List<RelevantBookInfoAI>();

            foreach (var stock in allBooks)
            {
                RelevantBookInfoAI book = new RelevantBookInfoAI()
                {
                    Title = stock.StcBok.BokTitle,
                    Sinopsis = stock.StcBok.BokSinopsis,
                    Edition = (int)stock.StcBok.BokEdition,
                    PagesAmount = (int)stock.StcBok.BokPagesAmount,
                    Year = (int)stock.StcBok.BokYear,
                    Publisher = stock.StcBok.BokPbl.PblName,
                    Author = stock.StcBok.BokBat.BatName,
                    Price = stock.StcSalePrice,
                    AvailableAmount = (int)stock.StcAvailableAmount,
                    Categories = new List<string>()
                };

                foreach (var category in stock.StcBok.BcrBcts)
                    book.Categories.Add(category.BctName);

                infos.Add(book);
            }

            return infos;
        }

        public Stock AddToStock(Stock stock, decimal amountAdded, bool addBlocked = false)
        {
            if (!addBlocked)
            {
                stock.StcAvailableAmount += amountAdded;
            } else
            {
                stock.StcBlockedAmount += amountAdded;
            }

            return _stockRepository.Update(stock);
        }
    }
}
