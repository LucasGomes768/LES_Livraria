using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR.RegionsR;

namespace E_CommerceLivraria.Services.AddressS.RegionsS {
    public class StateService : IStateService {
        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository) {
            _stateRepository = stateRepository;
        }

        public State CreateIfNew(State state, Country country) {
            var query = _stateRepository.GetAll();
            var result = query.Where(x => x.SttName.ToLower() == state.SttName.ToLower())
                .FirstOrDefault(x => x.SttCtr == country);

            if (!(result is State)) {
                state.SttCtrId = state.SttCtr.CtrId;
                return _stateRepository.Add(state);
            }
            else {
                return result;
            }
        }
    }
}
