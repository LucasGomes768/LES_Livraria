using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.StockR {
    public class StockRepository : IStockRepository {
        private readonly ECommerceDbContext _dbContext;

        public StockRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Stock? Get(decimal id) {
            return _dbContext.Stocks
                .Include(x => x.StcSpp)
                .Include(x => x.Book)
                    .ThenInclude(x => x.BokBat)
                .Include(x => x.Book)
                    .ThenInclude(x => x.BokPrg)
                .Include(x => x.Book)
                    .ThenInclude(x => x.BokPbl)
                .Include(x => x.Book)
                    .ThenInclude(x => x.BcrBcts)
                .FirstOrDefault(x => x.StcId == id);
        }

        public List<Stock> GetAll() {
            return _dbContext.Stocks
                .Include(x => x.StcSpp)
                .Include(x => x.StcBok)
                    .ThenInclude(x => x.BokBat)
                .Include(x => x.StcBok)
                    .ThenInclude(x => x.BokPrg)
                .Include(x => x.StcBok)
                    .ThenInclude(x => x.BokPbl)
                .Include(x => x.StcBok)
                    .ThenInclude(x => x.BcrBcts)
                .ToList();
        }

        public Stock? GetByBook(decimal id) {
            return _dbContext.Stocks
                .Include(x => x.StcSpp)
                .Include(x => x.StcBok)
                    .ThenInclude(x => x.BokBat)
                .Include(x => x.StcBok)
                    .ThenInclude(x => x.BokPrg)
                .Include(x => x.StcBok)
                    .ThenInclude(x => x.BokPbl)
                .Include(x => x.StcBok)
                    .ThenInclude(x => x.BcrBcts)
                .FirstOrDefault(x => x.StcBok.BokId == id);
        }

        public Stock Update(Stock stock) {
            var stc = Get(stock.StcId);

            if (stc == null) throw new Exception("Não existe um stock com esse id");

            _dbContext.Stocks.Update(stock);
            _dbContext.SaveChanges();

            return stock;
        }
    }
}
