using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.CreditCardR
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ECommerceDbContext _dbContext;

        public CreditCardRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CreditCard Create(CreditCard creditCard)
        {
            _dbContext.CreditCards.Add(creditCard);
            _dbContext.SaveChanges();

            return creditCard;
        }

        public bool Delete(decimal id)
        {
            var crd = Get(id);
            if (crd == null) throw new Exception("Cartão de crédito não foi encontrado");

            _dbContext.CreditCards.Remove(crd);
            _dbContext.SaveChanges();

            return true;
        }

        public CreditCard? Get(decimal id)
        {
            return _dbContext.CreditCards
                .Include(x => x.CrdCcf)
                .FirstOrDefault(x => x.CrdId == id);
        }

        public List<CreditCard> GetAll()
        {
            return _dbContext.CreditCards
                .Include(x => x.CrdCcf)
                .ToList();
        }

        public CreditCard Update(CreditCard creditCard)
        {
            var crd = Get(creditCard.CrdId);
            if (crd == null) throw new Exception("Cartão de crédito não foi encontrado");

            _dbContext.CreditCards.Update(creditCard);
            _dbContext.SaveChanges();

            return creditCard;
        }
    }
}
