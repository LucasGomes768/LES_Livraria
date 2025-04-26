using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR.RegionsR;

namespace E_CommerceLivraria.Services.AddressS.RegionsS {
    public class NeighborhoodService : INeighborhoodService{
        private readonly INeighborhoodRepository _neighborhoodRepository;

        public NeighborhoodService(INeighborhoodRepository neighborhoodRepository) {
            _neighborhoodRepository = neighborhoodRepository;
        }

        public Neighborhood CreateIfNew(Neighborhood neighborhood, City city) {
            var query = _neighborhoodRepository.GetAll();
            var result = query.Where(x => x.NbhName.ToLower() == neighborhood.NbhName.ToLower())
                .FirstOrDefault(x => x.NbhCty.CtyName.ToLower() == city.CtyName.ToLower());

            if (result == null) {
                neighborhood.NbhCtyId = neighborhood.NbhCty.CtyId;
                return _neighborhoodRepository.Add(neighborhood);
            } else {
                return result;
            }
        }
    }
}
