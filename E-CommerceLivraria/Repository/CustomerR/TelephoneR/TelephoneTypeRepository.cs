using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR.TelephoneR {
    public class TelephoneTypeRepository : ITelephoneTypeRepository{
        private readonly ECommerceDbContext _dbContext;

        public TelephoneTypeRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public TelephoneType Add(TelephoneType telephoneType) {
            _dbContext.TelephoneTypes.Add(telephoneType);
            _dbContext.SaveChanges();

            return telephoneType;
        }
        public TelephoneType? Get(decimal id) {
            return _dbContext.TelephoneTypes.Where(x => x.TptId == id).SingleOrDefault();
        }
        public List<TelephoneType> GetAll() {
            return _dbContext.TelephoneTypes.ToList();
        }
    }
}
