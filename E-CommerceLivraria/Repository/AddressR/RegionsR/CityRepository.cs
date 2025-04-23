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
            _dbContext.SaveChanges();

            _dbContext.Entry(city).State = EntityState.Detached;

            return _dbContext.Cities.AsNoTracking().FirstOrDefault(x => x.CtyId == city.CtyId);
        }

        public City? Get(decimal id) {
            return _dbContext.Cities.Where(x => x.CtyId == id).SingleOrDefault();
        }

        public List<City> GetAll() {
            return _dbContext.Cities.ToList();
        }
    }
}
