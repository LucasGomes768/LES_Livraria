using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.AddressR {
    public interface IAddressRepository {
        public Address Add(Address address);
        public bool Remove(decimal id);
        public Address? Get(decimal id);
        public List<Address> GetAll();
        public Address Update(Address address);
    }
}
