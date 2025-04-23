using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.AddressR {
    public class ResidenceTypeRepository : IResidenceTypeRepository{
        private readonly ECommerceDbContext _dbContext;

        public ResidenceTypeRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public ResidenceType? Get(decimal id) {
            return _dbContext.ResidenceTypes.Where(x => x.RstId == id).SingleOrDefault();
        }

        public List<ResidenceType> GetAll() {
            return _dbContext.ResidenceTypes.ToList();
        }
    }
}
