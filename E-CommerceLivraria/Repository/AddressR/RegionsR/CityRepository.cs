using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.AddressR.RegionsR {
    public class CityRepository : ICityRepository{
        private readonly ECommerceDbContext _dbContext;

        public CityRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public City Add(City city) {
            _dbContext.Cities.Add(city);

            return city;
        }

        public City? Get(decimal id) {
            return _dbContext.Cities
                .Include(x => x.CtyStt)
                .FirstOrDefault(x => x.CtyId == id);
        }

        public List<City> GetAll() {
            return _dbContext.Cities
                .Include(x => x.CtyStt)
                .ToList();
        }
    }
}
