using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CreditCardR
{
    public class CreditCardFlagsRepository : ICreditCardFlagsRepository
    {
        private readonly ECommerceDbContext _dbContext;

        public CreditCardFlagsRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CreditCardFlag? Get(decimal id)
        {
            return _dbContext.CreditCardFlags.FirstOrDefault(x => x.CcfId == id);
        }

        public List<CreditCardFlag> GetAll()
        {
            return _dbContext.CreditCardFlags.ToList();
        }
    }
}
