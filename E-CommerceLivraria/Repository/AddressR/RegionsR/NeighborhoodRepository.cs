using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.AddressR.RegionsR {
    public class NeighborhoodRepository : INeighborhoodRepository{
        private readonly ECommerceDbContext _dbContext;

        public NeighborhoodRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Neighborhood Add(Neighborhood neighborhood) {
            _dbContext.Neighborhoods.Add(neighborhood);
            _dbContext.SaveChanges();

            _dbContext.Entry(neighborhood).State = EntityState.Detached;

            return _dbContext.Neighborhoods.AsNoTracking().FirstOrDefault(x => x.NbhId == neighborhood.NbhId);
        }

        public Neighborhood? Get(decimal id) {
            return _dbContext.Neighborhoods.Where(x => x.NbhId == id).SingleOrDefault();
        }

        public List<Neighborhood> GetAll() {
            return _dbContext.Neighborhoods.ToList();
        }
    }
}
