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

            return neighborhood;
        }

        public Neighborhood? Get(decimal id) {
            return _dbContext.Neighborhoods
                .Include(x => x.NbhCty)
                .FirstOrDefault(x => x.NbhId == id);
        }

        public List<Neighborhood> GetAll() {
            return _dbContext.Neighborhoods
                .Include(x => x.NbhCty)
                .ToList();
        }
    }
}
