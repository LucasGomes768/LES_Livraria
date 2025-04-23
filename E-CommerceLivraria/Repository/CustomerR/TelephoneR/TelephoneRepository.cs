using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.CustomerR.TelephoneR {
    public class TelephoneRepository : ITelephoneRepository{
        private readonly ECommerceDbContext _dbContext;

        public TelephoneRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Telephone Add(Telephone telephone) {
            _dbContext.Telephones.Add(telephone);
            _dbContext.SaveChanges();

            return telephone;
        }

        public Telephone? Get(decimal id) {
            return _dbContext.Telephones
                .Include(x => x.TlpTpt)
                .FirstOrDefault(x => x.TlpTptId == id);
        }

        public void Remove(Telephone tlp) {
            _dbContext.Telephones.Remove(tlp);
            _dbContext.SaveChanges();
        }
    }
}
