using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.AddressR {
    public class AddressRepository : IAddressRepository {
        private readonly ECommerceDbContext _dbContext;

        public AddressRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Address Add(Address address) {
            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();

            return address;
        }

        public Address? Get(decimal id, bool tracked = true) {
            if (tracked)
            {
                return _dbContext.Addresses
                    .Include(x => x.AddRst)
                    .Include(x => x.AddPpt)
                    .Include(x => x.AddNbh)
                        .ThenInclude(x => x.NbhCty)
                            .ThenInclude(x => x.CtyStt)
                                .ThenInclude(x => x.SttCtr)
                    .Include(x => x.BadCtms)
                    .Include(x => x.DadCtms)
                    .FirstOrDefault(x => x.AddId == id);
            } else
            {
                return _dbContext.Addresses
                    .AsNoTracking()
                    .Include(x => x.AddRst)
                    .Include(x => x.AddPpt)
                    .Include(x => x.AddNbh)
                        .ThenInclude(x => x.NbhCty)
                            .ThenInclude(x => x.CtyStt)
                                .ThenInclude(x => x.SttCtr)
                    .Include(x => x.BadCtms)
                    .Include(x => x.DadCtms)
                    .FirstOrDefault(x => x.AddId == id);
            }
        }

        public List<Address> GetAll() {
            return _dbContext.Addresses.ToList();
        }

        public bool Delete(decimal id) {
            var add = Get(id);

            if (add == null) throw new System.Exception("Um endereço com este ID não foi encontrado");

            _dbContext.Addresses.Remove(add);
            _dbContext.SaveChanges();

            return true;
        }

        public Address Update(Address address) {
            var add = Get(address.AddId);

            if (add == null) throw new System.Exception("Um endereço com este ID não foi encontrado");
            _dbContext.Entry(add).State = EntityState.Detached;
            _dbContext.Entry(add.AddNbh).State = EntityState.Detached;

            _dbContext.Addresses.Update(address);
            _dbContext.SaveChanges();

            return address;
        }
    }
}
