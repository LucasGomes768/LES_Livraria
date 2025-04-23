using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR.GenderR {
    public class GenderRepository : IGenderRepository {
        private readonly ECommerceDbContext _dbContext;

        public GenderRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Gender? Get(decimal id) {
            return _dbContext.Genders.FirstOrDefault(x => x.GndId == id);
        }

        public List<Gender> GetAll() {
            return _dbContext.Genders.ToList();
        }
    }
}
