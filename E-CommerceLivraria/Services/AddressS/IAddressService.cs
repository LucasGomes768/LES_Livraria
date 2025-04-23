using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.AddressS {
    public interface IAddressService {
        public Address Create(Address address);
        public bool Remove(decimal id);
        public Address? Get(decimal id);
        public List<Address> GetAll();
        public Address Update(Address address);
    }
}
