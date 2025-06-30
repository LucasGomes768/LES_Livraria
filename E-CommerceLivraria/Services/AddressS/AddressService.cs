using E_CommerceLivraria.DTO.ProfileDTO.AddressDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Services.AddressS.RegionsS;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        public Address? Get(decimal id, bool tracked = true) {
            return _addressRepository.Get(id, tracked);
        }

        public List<Address> GetAll() {
            return _addressRepository.GetAll();
        }

        public bool AccountRemove(Address add, Customer ctm, EAddressType list) {
            if (list == EAddressType.BILLING)
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

        public Address Update(DetailedAddDTO dto)
        {
            var address = _addressRepository.Get(dto.Id, true);
            if (address == null) throw new Exception("O endereço não foi encontrado");

            UpdateAddressProperties(address, dto);
            UpdateLocationHierarchy(address, dto);

            return _addressRepository.Update(address);
        }

        private void UpdateAddressProperties(Address address, DetailedAddDTO dto)
        {
            if (dto.PublicPlace != null) address.AddPublicPlace = dto.PublicPlace;
            if (dto.Number != null) address.AddNumber = (decimal)dto.Number;
            if (dto.Cep != null) address.AddCepStyled = dto.Cep;
            if (dto.ShortPhrase != null) address.AddShortPhrase = dto.ShortPhrase;
            if (dto.PublicPlaceType != null) address.AddPptId = dto.PublicPlaceType.Value;
            if (dto.ResidenceType != null) address.AddRstId = dto.ResidenceType.Value;
            if (dto.Observations != null) address.AddObservations = dto.Observations;
        }

        private void UpdateLocationHierarchy(Address address, DetailedAddDTO dto)
        {
            address.AddNbh ??= new Neighborhood();
            address.AddNbh.NbhCty ??= new City();
            address.AddNbh.NbhCty.CtyStt ??= new State();
            address.AddNbh.NbhCty.CtyStt.SttCtr ??= new Country();

            if (dto.Neighborhood != null) address.AddNbh.NbhName = dto.Neighborhood;
            if (dto.City != null) address.AddNbh.NbhCty.CtyName = dto.City;
            if (dto.State != null) address.AddNbh.NbhCty.CtyStt.SttName = dto.State;
            if (dto.Country != null) address.AddNbh.NbhCty.CtyStt.SttCtr.CtrName = dto.Country;
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
