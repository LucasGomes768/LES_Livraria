using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.AddressR.RegionsR {
    public class CountryRepository : ICountryRepository{
        private readonly ECommerceDbContext _dbContext;

        public CountryRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Country Add(Country country) {
            _dbContext.Countries.Add(country);

            return country;
        }

        public Country? Get(decimal id) {
            return _dbContext.Countries.FirstOrDefault(x => x.CtrId == id);
        }

        public List<Country> GetAll() {
            return _dbContext.Countries.ToList();
        }
    }
}
