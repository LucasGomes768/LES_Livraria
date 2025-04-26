using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.AddressR.RegionsR {
    public class StateRepository : IStateRepository{
        private readonly ECommerceDbContext _dbContext;

        public StateRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public State Add(State state) {
            _dbContext.States.Add(state);

            return state;
        }

        public State? Get(decimal id) {
            return _dbContext.States
                .Include(x => x.SttCtr)
                .FirstOrDefault(x => x.SttId == id);
        }

        public List<State> GetAll() {
            return _dbContext.States
                .Include(x => x.SttCtr)
                .ToList();
        }
    }
}
