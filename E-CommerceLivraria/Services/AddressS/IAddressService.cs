using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.AddressS {
    public interface IAddressService {
        public Address Create(Address address);
        public bool AccountRemove(Address add, Customer ctm, string list);
        public Address? Get(decimal id);
        public List<Address> GetAll();
        public Address Update(Address address);
    }
}
