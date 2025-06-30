using E_CommerceLivraria.DTO.ProfileDTO.AddressDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.AddressS {
    public interface IAddressService {
        public Address Create(Address address);
        public bool AccountRemove(Address add, Customer ctm, EAddressType list);
        public Address? Get(decimal id, bool tracked = true);
        public List<Address> GetAll();
        public Address Update(Address address);
        public Address Update(DetailedAddDTO dto);
    }
}
