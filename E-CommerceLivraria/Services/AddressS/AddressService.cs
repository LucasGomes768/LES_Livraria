using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Services.AddressS.RegionsS;

namespace E_CommerceLivraria.Services.AddressS {
    public class AddressService : IAddressService{
        private readonly IAddressRepository _addressRepository;
        private readonly INeighborhoodService _neighborhoodService;
        private readonly ICityService _cityService;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IPublicPlaceTypeRepository _publicPlaceTypeRepository;
        private readonly IResidenceTypeRepository _residenceTypeRepository;

        public AddressService(IAddressRepository addressRepository,
            INeighborhoodService neighborhoodService, ICityService cityService, IStateService stateService,
            ICountryService countryService, IPublicPlaceTypeRepository publicPlaceTypeRepository,
            IResidenceTypeRepository residenceTypeRepository) {

            _addressRepository = addressRepository;
            _neighborhoodService = neighborhoodService;
            _cityService = cityService;
            _stateService = stateService;
            _countryService = countryService;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
            _residenceTypeRepository = residenceTypeRepository;
        }

        public Address Create(Address address) {
            Country ctr = address.AddNbh.NbhCty.CtyStt.SttCtr;
            address.AddNbh.NbhCty.CtyStt.SttCtr = _countryService.CreateIfNew(ctr);

            State stt = address.AddNbh.NbhCty.CtyStt;
            address.AddNbh.NbhCty.CtyStt = _stateService.CreateIfNew(stt, ctr);

            City cty = address.AddNbh.NbhCty;
            address.AddNbh.NbhCty = _cityService.CreateIfNew(cty, stt);

            address.AddNbh = _neighborhoodService.CreateIfNew(address.AddNbh, cty);
            address.AddNbhId = address.AddNbh.NbhId;

            var pptTemp = _publicPlaceTypeRepository.Get(address.AddPptId);
            address.AddPpt = pptTemp;

            var rstTemp = _residenceTypeRepository.Get(address.AddRstId);
            address.AddRst = rstTemp;

            Random rng = new Random();
            address.AddShipping = (decimal)rng.NextDouble() * 200;

            return _addressRepository.Add(address);
        }

        public Address? Get(decimal id) {
            return _addressRepository.Get(id);
        }

        public List<Address> GetAll() {
            return _addressRepository.GetAll();
        }

        public bool AccountRemove(Address add, Customer ctm, string list) {
            if (list == "billing")
            {
                var ctmHas = add.BadCtms.FirstOrDefault(x => x.CtmId == ctm.CtmId);
                if (ctmHas == null) return false;

                add.BadCtms.Remove(ctm);
                _addressRepository.Update(add);
            } else
            {
                var ctmHas = add.DadCtms.FirstOrDefault(x => x.CtmId == ctm.CtmId);
                if (ctmHas == null) return false;

                add.DadCtms.Remove(ctm);
                _addressRepository.Update(add);
            }

            return true;
        }

        public Address Update(Address address) {
            Country ctr = address.AddNbh.NbhCty.CtyStt.SttCtr;
            address.AddNbh.NbhCty.CtyStt.SttCtr = _countryService.CreateIfNew(ctr);

            State stt = address.AddNbh.NbhCty.CtyStt;
            address.AddNbh.NbhCty.CtyStt = _stateService.CreateIfNew(stt, ctr);

            City cty = address.AddNbh.NbhCty;
            address.AddNbh.NbhCty = _cityService.CreateIfNew(cty, stt);

            address.AddNbh = _neighborhoodService.CreateIfNew(address.AddNbh, cty);
            address.AddNbhId = address.AddNbh.NbhId;

            var pptTemp = _publicPlaceTypeRepository.Get(address.AddPptId);
            address.AddPpt = pptTemp;

            var rstTemp = _residenceTypeRepository.Get(address.AddRstId);
            address.AddRst = rstTemp;

            return _addressRepository.Update(address);
        }
    }
}
